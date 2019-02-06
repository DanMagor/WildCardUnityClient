using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ClientManager : MonoBehaviour {

    [SerializeField] private string ipAddress;
    [SerializeField] private int port;


    

    public static Text enemyUsernameLabel;
    public static Text playerUsernameLabel;

    public static string playerUsername;
    public static string enemyUsername;



    private void Awake()
    {
        DontDestroyOnLoad(this);
        UnityThread.initUnityThread();

        ClientHandleData.InitializePacketListener();
        ClientTCP.InitializeClientSocket(ipAddress, port);
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }



   

    public static void LoadMatch(int matchID)
    {
        SceneManager.sceneLoaded += InitializeLabels;
        SceneManager.sceneLoaded += SetReadyForMatch;
        PlayerMatchManager.matchID = matchID;
        SceneManager.LoadScene("Match");
        
    }
    private static void InitializeLabels(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Match")
        {
            playerUsernameLabel = GameObject.Find("Canvas/PlayerUsernameLabel").GetComponent<Text>();
            enemyUsernameLabel = GameObject.Find("Canvas/EnemyUsernameLabel").GetComponent<Text>();
            playerUsernameLabel.text = playerUsername;
            enemyUsernameLabel.text = enemyUsername;

           


        }

    }


    //TODO: Think about saving in JSON. Rewrite?
    public static void SaveCards(byte[] data)
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

            string json = JsonUtility.ToJson(card);

            string path = Application.dataPath + @"\Resources\Cards";

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            System.IO.File.WriteAllText(path + @"\Card" + cardID, json);
                

            //Debug.Log("Creating Asset Card" + cardID);
            //CardScriptableObject asset = ScriptableObject.CreateInstance<CardScriptableObject>(); //TODO MOVE LATER

            //asset.ID = cardID;
            //asset.damageLabel = damage.ToString();
            //asset.bulletLabel = bullet.ToString();
            //Debug.Log("Creating Asset2 Card" + cardID);
            //AssetDatabase.CreateAsset(asset, "Assets/Resources/Cards/Card" + cardID + ".asset");
            //Debug.Log("Saving Asset Card" + cardID);
            //AssetDatabase.SaveAssets();
        }
    }

    //TODO: Check Architecture Later
    private static void SetReadyForMatch(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Match")
        {
            ClientTCP.PACKAGE_SetReadyForMatch(PlayerMatchManager.matchID);
        }
    }

   
    

}
