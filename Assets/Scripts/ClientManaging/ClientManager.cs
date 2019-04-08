using System;
using System.Collections.Generic;
using Assets.Prefabs.DataSaving;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientManager : MonoBehaviour
{
    public static Dictionary<int, CardInstanceSerializable> allCardsInfo = new Dictionary<int, CardInstanceSerializable>();

    public static Dictionary<int, Sprite> allCardsSprites = new Dictionary<int, Sprite>();
    public static Dictionary<int, Sprite> allEffectsSprites = new Dictionary<int, Sprite>();
    public static Dictionary<int, Sprite> directionSprites;



    private ClientMatchManager currentMatchManager;


    public string playerUsername;

    [SerializeField] private string ipAddress = "127.0.0.1";
    [SerializeField] private int port = 5555;



    private clientMatchManagerInfo matchInfo;
    private struct clientMatchManagerInfo
    {
        public int matchID;
        public string playerUsername;
        public string enemyUsername;
    }

    private void Awake()
    {
        // This two methods for working with TCP in Unity Thread
        DontDestroyOnLoad(this);
        UnityThread.initUnityThread();

        //Connection
        ClientHandleData.InitializePacketListener();
        ClientTCP.InitializeClientSocket(ipAddress, port);

        directionSprites = new Dictionary<int, Sprite>()
        {

        {0, Resources.Load<Sprite>("Effects/LeftArrow") },
        {1, Resources.Load<Sprite>("Effects/RightArrow") }

        };
    }

    public void InitializeClientManager()
    {
        // This two methods for working with TCP in Unity Thread
        DontDestroyOnLoad(this);
        UnityThread.initUnityThread();

        //Connection
        ClientHandleData.InitializePacketListener();
        ClientTCP.InitializeClientSocket(ipAddress, port);

        directionSprites = new Dictionary<int, Sprite>()
        {

            {0, Resources.Load<Sprite>("Effects/LeftArrow") },
            {1, Resources.Load<Sprite>("Effects/RightArrow") }

        };
    }


    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.R)) ClientTCP.PACKAGE_SendRestartMatch(currentMatchManager.matchID);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadMatch(byte[] data)
    {
      
        //if (currentMatchManager != null)
        //{
        //    Debug.LogError("MatchManager is not null!");
        //    throw new Exception("MatchManager is not null!");

        //}
        
        var buffer = new ByteBuffer();
        currentMatchManager = GameObject.FindObjectOfType<ClientMatchManager>();


        buffer.WriteBytes(data);
        buffer.ReadInteger(); // Read Package ID


        matchInfo.matchID = buffer.ReadInteger();

        matchInfo.playerUsername = buffer.ReadString();
        matchInfo.enemyUsername = buffer.ReadString();


        SceneManager.sceneLoaded += MatchSceneLoaded;
        SceneManager.LoadScene("Match");
    }

    private void MatchSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        currentMatchManager = FindObjectOfType<ClientMatchManager>();
        currentMatchManager.matchID = matchInfo.matchID;
        currentMatchManager.PlayerEntityController.userName = matchInfo.playerUsername;
        currentMatchManager.EnemyEntityController.userName = matchInfo.enemyUsername;
        ClientHandleData.clientMatchManager = currentMatchManager;
        currentMatchManager.SendSetReady();

        SceneManager.sceneLoaded -= MatchSceneLoaded;

    }

   

    //TODO: Think about saving in JSON. Rewrite? DO we need to save them in files?
    public void SaveAllCardsData(byte[] data)
    {
        var buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger(); //Read Package ID

        //On First Iteration Read Attack Cards
        var numberOfCards = buffer.ReadInteger(); //Read number of Cards

        for (var i = 0; i < numberOfCards; i++)
        {
            var cardInstance = new CardInstanceSerializable();


            //TODO: Delete Later unneeded fields for client
            cardInstance.ID = buffer.ReadInteger();
            cardInstance.Name = buffer.ReadString();
            cardInstance.Type = buffer.ReadString();
            cardInstance.IsComboCard = buffer.ReadBool();
            cardInstance.NForCombo = buffer.ReadInteger();
            cardInstance.ComboCards = new List<int>();
            for (var j = 0; j < cardInstance.NForCombo; j++) cardInstance.ComboCards.Add(buffer.ReadInteger());

            cardInstance.CardImage = buffer.ReadString();
            cardInstance.ItemImage = buffer.ReadString();
            cardInstance.Value = buffer.ReadInteger();
            cardInstance.Animation = buffer.ReadString();

            allCardsInfo[cardInstance.ID] = cardInstance;

            allCardsSprites[cardInstance.ID] = Resources.Load<Sprite>("Cards/" + cardInstance.CardImage);
            allEffectsSprites[cardInstance.ID] = Resources.Load<Sprite>("Effects/" + cardInstance.ItemImage);

            ///////////////////////////// TO DO : CHECK DO I NEED IT \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            ////Saving to JSON. DO we need it?
            //string json = JsonUtility.ToJson(card);

            //string path = Application.dataPath + @"\Resources\Cards";

            //if (!System.IO.Directory.Exists(path))
            //{
            //    System.IO.Directory.CreateDirectory(path);
            //}
            //System.IO.File.WriteAllText(path + @"\Card" + card.id, json);
            ///////////////////////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        }
    }






    public static void RequestSearch()
    {
        ClientTCP.PACKAGE_SearchOpponent();
    }


    //private void InitializeLabels(Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.name == "Match")
    //    {
    //        //Find Player MatchManager on the scene for ClientHandleData
    //        //Have to search because we load new Scene
    //        currentMatchManager = GameObject.Find("ClientMatchManager").GetComponent<ClientMatchManager>();
    //        currentMatchManager.matchID = currentMatchID;

    //        //Have to assign it here, cause we can find it only when level is alredy loaded
    //        ClientHandleData.clientMatchManager = currentMatchManager;
    //        ClientTCP.clientMatchManager = currentMatchManager;


    //        //Initialize Labels:
    //        currentMatchManager.InitializeLabels(playerUsername, enemyUsername);
    //    }
    //}
}