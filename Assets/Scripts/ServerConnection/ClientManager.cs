using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ClientManager : MonoBehaviour {

    [SerializeField] private string ipAddress;
    [SerializeField] private int port;

    


    private PlayerMatchManager currentMatchManager;

    public int currentMatchID;
    public  string playerUsername;
    public  string enemyUsername;


    public static Dictionary<int,CardSerializable> allCardsInfo = new Dictionary<int, CardSerializable>();
    public static Dictionary<int, Sprite> allCardsSprites = new Dictionary<int, Sprite>();



    private void Awake()
    {
        // This two methods for working with TCP in Unity Thread
        DontDestroyOnLoad(this);
        UnityThread.initUnityThread();

        //Connection
        ClientHandleData.InitializePacketListener();
        ClientTCP.InitializeClientSocket(ipAddress, port);
    }

    public  void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public  void LoadMatch(int matchID)
    {
        SceneManager.sceneLoaded += InitializeLabels;
        SceneManager.sceneLoaded += SetReadyForMatch;
        
        //Have to save it here, because we need to download level first.-
        currentMatchID = matchID;
        SceneManager.LoadScene("Match");
        
    }

    //TODO: Think about saving in JSON. Rewrite? DO we need to save them in files?
    public  void SaveCards(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packageID = buffer.ReadInteger();
        int numberOfCards = buffer.ReadInteger();



        for (int i = 0; i < numberOfCards; i++)
        {

            int cardID = buffer.ReadInteger();
            int damage = buffer.ReadInteger();
            int bullet = buffer.ReadInteger();
            string image = buffer.ReadString();

            var card = new CardSerializable();
            card.id = cardID;
            card.damage = damage;
            card.bullet = bullet;
            card.image = image;


            //Add to all cards dictionary
            allCardsInfo.Add(cardID, card);

            //Add all sprites according to ID
            Sprite im = Resources.Load<Sprite>(@"Cards\" + image);
            allCardsSprites.Add(cardID, im);


            //Saving to JSON. DO we need it?
            string json = JsonUtility.ToJson(card);

            string path = Application.dataPath + @"\Resources\Cards";

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            System.IO.File.WriteAllText(path + @"\Card" + cardID, json);
        }
    }

    //TODO: Check Architecture Later
    private  void SetReadyForMatch(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Match")
        {
            ClientTCP.PACKAGE_SetReadyForMatch(currentMatchManager.matchID);
        }
    }
    private void InitializeLabels(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Match")
        {
            

            //Find Player MatchManager on the scene for ClientHandleData
            //Have to search because we load new Scene
            currentMatchManager = GameObject.Find("PlayerMatchManager").GetComponent<PlayerMatchManager>();
            currentMatchManager.matchID = currentMatchID;

            //Have to assign it here, cause we can find it only when level is alredy loaded
            ClientHandleData.playerMatchManager = currentMatchManager;
            ClientTCP.playerMatchManager = currentMatchManager;


            //Initialize Labels:
            currentMatchManager.InitializeLabels(playerUsername, enemyUsername);



        }

    }






}
