using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ClientManager : MonoBehaviour
{

    [SerializeField] private string ipAddress;
    [SerializeField] private int port;




    private PlayerMatchManager currentMatchManager;

    public int currentMatchID;
    public string playerUsername;
    public string enemyUsername;


   // public static Dictionary<int, CardSerializable> allCardsInfo = new Dictionary<int, CardSerializable>();
    public static Dictionary<int, Sprite> allCardsSprites = new Dictionary<int, Sprite>();
    public static Dictionary<int, EffectSerializable> allEffectsInfo = new Dictionary<int, EffectSerializable>();
    public static Dictionary<int, Sprite> allEffectsSprites = new Dictionary<int, Sprite>();




    private void Awake()
    {
        // This two methods for working with TCP in Unity Thread
        DontDestroyOnLoad(this);
        UnityThread.initUnityThread();

        //Connection
        ClientHandleData.InitializePacketListener();
        ClientTCP.InitializeClientSocket(ipAddress, port);
    }


    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            ClientTCP.PACKAGE_SendRestartMatch(currentMatchID);
        }
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadMatch(int matchID)
    {
        SceneManager.sceneLoaded += InitializeLabels;
        SceneManager.sceneLoaded += SetReadyForMatch;

        //Have to save it here, because we need to download level first.-
        currentMatchID = matchID;
        SceneManager.LoadScene("Match");

    }

    //TODO: Think about saving in JSON. Rewrite? DO we need to save them in files?
    public void SaveCardsAndEffects(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packageID = buffer.ReadInteger();

        //On First Iteration Read Attack Cards
        int numberOfCards = buffer.ReadInteger(); //Read number of Attack Cards
        for (int i = 0; i < numberOfCards; i++)
        {

            //var card = new CardSerializable();

            ////Genereal Info
            //card.id = buffer.ReadInteger();
            //card.type = buffer.ReadString();
            //card.name = buffer.ReadString();
            //card.image = buffer.ReadString();

            ////Attack Card Info
            //card.damage = buffer.ReadInteger();
            //card.bullets = buffer.ReadInteger();
            //card.accuracy = buffer.ReadInteger();

            ////Initiative Effect
            //card.initiativeEffect = buffer.ReadInteger();
            //card.initiativeValue = buffer.ReadInteger();
            //card.initiativeDuration = buffer.ReadInteger();


            ////Add to all cards dictionary
            //allCardsInfo.Add(card.id, card);

            ////Add all sprites according to ID
            //Sprite im = Resources.Load<Sprite>(@"Cards\" + card.image);
            //allCardsSprites.Add(card.id, im);



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

        //Second Heal Cards
        numberOfCards = buffer.ReadInteger(); //Read number of Heal Cards
        for (int i = 0; i < numberOfCards; i++)
        {

            //var card = new CardSerializable();

            ////Genereal Info
            //card.id = buffer.ReadInteger();
            //card.type = buffer.ReadString();
            //card.name = buffer.ReadString();
            //card.image = buffer.ReadString();

            ////Heal Card Info
            //card.heal = buffer.ReadInteger();

            ////Initiative Effect
            //card.initiativeEffect = buffer.ReadInteger();
            //card.initiativeValue = buffer.ReadInteger();
            //card.initiativeDuration = buffer.ReadInteger();



            ////Add to all cards dictionary
            //allCardsInfo.Add(card.id, card);

            ////Add all sprites according to ID
            //Sprite im = Resources.Load<Sprite>(@"Cards\" + card.image);
            //allCardsSprites.Add(card.id, im);



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

        //Item Cards
        numberOfCards = buffer.ReadInteger(); //Read number of Item Cards
        for (int i = 0; i < numberOfCards; i++)
        {

            //var card = new CardSerializable();

            ////Genereal Info
            //card.id = buffer.ReadInteger();
            //card.type = buffer.ReadString();
            //card.name = buffer.ReadString();
            //card.image = buffer.ReadString();

            ////Item Card Info
            //card.itemDuration = buffer.ReadInteger();
            //card.itemEffectLabel = buffer.ReadString();
            //card.itemEffectImage = buffer.ReadString();

            ////Initiative Effect
            //card.initiativeEffect = buffer.ReadInteger();
            //card.initiativeValue = buffer.ReadInteger();
            //card.initiativeDuration = buffer.ReadInteger();



            ////Add to all cards dictionary
            //allCardsInfo.Add(card.id, card);

            ////Add all sprites according to ID
            //Sprite im = Resources.Load<Sprite>(@"Cards\" + card.image);
            //allCardsSprites.Add(card.id, im);



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


        //Now Save Effects
        int numberOfEffects = buffer.ReadInteger();
        for (int i = 0; i < numberOfEffects; i++)
        {
            var effect = new EffectSerializable();
            effect.ID = buffer.ReadInteger();
            effect.name = buffer.ReadString();
            effect.image = buffer.ReadString();
            allEffectsInfo[effect.ID] = effect;
            allEffectsSprites.Add(effect.ID, Resources.Load<Sprite>(@"Effects\" + effect.image));
        }

        
    }

   

    //TODO: Check Architecture Later
    private void SetReadyForMatch(Scene scene, LoadSceneMode mode)
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
