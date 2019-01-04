using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClientManager : MonoBehaviour {

    [SerializeField] private string ipAddress;
    [SerializeField] private int port;
  //  [SerializeField] private PlayerMatchManager playerMM;


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

    //TODO: Check Architecture Later
    private static void SetReadyForMatch(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Match")
        {
            ClientTCP.PACKAGE_SetReadyState(PlayerMatchManager.matchID);
        }
    }

}
