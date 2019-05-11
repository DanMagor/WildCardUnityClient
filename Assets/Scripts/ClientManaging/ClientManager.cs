using System;
using System.Collections.Generic;
using Assets.Prefabs.DataSaving;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientManager : MonoBehaviour
{

    //Connection Info
    [SerializeField] private string ipAddress = "127.0.0.1";
    [SerializeField] private int port = 5555;

    private MatchManager currentMatchManager;
    //For temporary saving matchInfo between scene switching
    private clientMatchManagerInfo matchInfo;
    private struct clientMatchManagerInfo
    {
        public int matchID;
        public string playerUsername;
        public string enemyUsername;
    }

    public static Dictionary<int, CardEntity> AllCardsInfo = new Dictionary<int, CardEntity>();
    public static Dictionary<int, Sprite> AllCardsSprites = new Dictionary<int, Sprite>();
    public static Dictionary<int, Sprite> AllItemsSprites = new Dictionary<int, Sprite>();
    public static Dictionary<int, Sprite> DirectionSprites; //Will be loaded in Initialization

    public string PlayerUsername; //we need this info in the whole game //TODO: Add other info for player. Currency, e.t.c

    public void Awake()
    {
        InitializeClientManager();
    }
    public void Update()
    {
    }

    public void InitializeClientManager()
    {
        // This two methods for working with TCP in Unity Thread
        DontDestroyOnLoad(this);
        UnityThread.initUnityThread();

        //Connection
        ClientHandleData.InitializePacketListener();
        ClientTCP.InitializeClientSocket(ipAddress, port);

        DirectionSprites = new Dictionary<int, Sprite>()
        {

            {0, Resources.Load<Sprite>("Effects/LeftArrow") },
            {1, Resources.Load<Sprite>("Effects/RightArrow") }

        };
    }

    #region Scene Loading
    public void LoadMenu(byte[] data)
    {
        var buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger(); // Read Package ID


        var playerUsername = buffer.ReadString();
        PlayerUsername = playerUsername;

        SceneManager.LoadScene("Main Menu");
    }
    public void LoadMatch(byte[] data)
    {

        //if (currentMatchManager != null) //For checking if player is in match
        //{
        //    Debug.LogError("MatchManager is not null!");
        //    throw new Exception("MatchManager is not null!");

        //}

        var buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger(); // Read Package ID


        //Save info for scene loading
        matchInfo.matchID = buffer.ReadInteger();
        matchInfo.playerUsername = buffer.ReadString();
        matchInfo.enemyUsername = buffer.ReadString();

        //Call method after scene loading for initialization 
        SceneManager.sceneLoaded += MatchSceneLoaded;

        SceneManager.LoadScene("Match");
    }
    private void MatchSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        currentMatchManager = FindObjectOfType<MatchManager>();

        //Assign Match Info
        currentMatchManager.MatchId = matchInfo.matchID;
        currentMatchManager.PlayerEntityController.Username = matchInfo.playerUsername;
        currentMatchManager.EnemyEntityController.Username = matchInfo.enemyUsername;

        //Notify Data Handler about MatchManager for direct call
        ClientHandleData.matchManager = currentMatchManager;
        
        //Notify server that client is ready
        currentMatchManager.SendSetReady();

        //Remove callback from queue
        SceneManager.sceneLoaded -= MatchSceneLoaded;

    }
    #endregion


    #region Data Saving/Loading
    public void SaveAllCardsData(byte[] data)
    {
        var buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger(); //Read Package ID

        //On First Iteration Read Attack Cards
        var numberOfCards = buffer.ReadInteger(); //Read number of Cards

        for (var i = 0; i < numberOfCards; i++)
        {
            var cardInstance = new CardEntity();

            //TODO: Delete Later unneeded fields for client
            cardInstance.Id = buffer.ReadInteger();
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

            AllCardsInfo[cardInstance.Id] = cardInstance;

            AllCardsSprites[cardInstance.Id] = Resources.Load<Sprite>("Cards/" + cardInstance.CardImage);
            AllItemsSprites[cardInstance.Id] = Resources.Load<Sprite>("Effects/" + cardInstance.ItemImage);
        }
    }
    #endregion

    public static void RequestSearch()
    {
        ClientTCP.PACKAGE_SearchOpponent();
    }


}