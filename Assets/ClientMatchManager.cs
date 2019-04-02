using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Assets.Prefabs.DataSaving;
using UnityEngine;
using UnityEngine.UI; //TODO: Delete

public class ClientMatchManager : MonoBehaviour
{


    #region RoundResults

    public bool amIShot;
    public List<int> PlayerSoloCards;
    public List<List<int>> PlayerComboCards;
    public List<int> PlayerNotSelectedCards;

    public int PlayerHP;
    public int PlayerArmor;
    public List<int> EnemySelectedCards;
    public int EnemyHP;
    public int EnemyArmor;

    

    //TODO: Move to UI Class
    public Text playerHPLabel;
    public Text playerArmorLabel;

    public Text enemyHPLabel;
    public Text enemyArmorLabel;



    #endregion

    [NonSerialized]
    public bool Ready = false;

    public CardInstanceSerializable[] Cards;

    public int matchID;

    public AnimationManager AnimationManager;
    public CowboyEntityController PlayerEntityController;
    public CowboyEntityController EnemyEntityController;

    private void Awake()
    {
        AnimationManager = GetComponent<AnimationManager>();
       
    }

    private void Start()
    {
        SendSetReady();
    }

    public void ShowResult(ByteBuffer buffer)
    {

        buffer.ReadInteger();
        amIShot = buffer.ReadBool();

        PlayerSoloCards = new List<int>();
        int length = buffer.ReadInteger();
        for (int i = 0; i < length; i++)
        {
            PlayerSoloCards.Add(buffer.ReadInteger());
        }

        PlayerComboCards = new List<List<int>>();
        length = buffer.ReadInteger();
        for (int i = 0; i < length; i++)
        {
            List<int> temp = new List<int>();
            int length2 = buffer.ReadInteger();;
            for (int j = 0; j < length2; j++)
            {
                temp.Add(buffer.ReadInteger());
            }
            PlayerComboCards.Add(temp);
        }

        PlayerNotSelectedCards = new List<int>();
        length = buffer.ReadInteger();
        for (int i = 0; i < length; i++)
        {
            PlayerNotSelectedCards.Add(buffer.ReadInteger());
        }
        

        PlayerHP = buffer.ReadInteger();
        PlayerArmor = buffer.ReadInteger();

        //TODO: Move to UI class
        playerHPLabel.text = PlayerHP.ToString();
        playerArmorLabel.text = PlayerArmor.ToString();

        EnemySelectedCards = new List<int>();
        length = buffer.ReadInteger();
        for (int i = 0; i < length; i++)
        {
            EnemySelectedCards.Add(buffer.ReadInteger());
        }

        EnemyHP = buffer.ReadInteger();
        EnemyArmor = buffer.ReadInteger();

        //TODO: Move to UI class
        enemyHPLabel.text = EnemyHP.ToString();
        enemyArmorLabel.text = EnemyArmor.ToString();





        AnimationManager.ShowResult();


        
       
        
    }

    

    public void SendSetReady()
    {
       ClientTCP.PACKAGE_SetReady(matchID);
    }


    public void HandleSendedCards(ByteBuffer data)
    {
       
        data.ReadInteger(); //Read package ID

        var numberOfCards = data.ReadInteger();

        
        Cards = new CardInstanceSerializable[numberOfCards];

        for (var i = 0; i < numberOfCards; i++)
        {
            Cards[i] = ClientManager.allCardsInfo[data.ReadInteger()];
            Cards[i].direction = data.ReadInteger();
        }
    }


    public void StartRound(ByteBuffer data)
    {
        data.ReadInteger();//Read Package ID
        StartTimer(data.ReadFloat());
    }
    private void StartTimer(float timerTime)
    {
        Debug.LogFormat("Timer {0} sec", timerTime);
    }

    public void ShowCards(ByteBuffer data)
    {
        AnimationManager.ShowCards();
    }

    public void SendToggleCard(int position)
    {
      ClientTCP.PACKAGE_Match_ToggleCard(matchID, position);
    }

    public void ToggleCard(ByteBuffer data)
    {
        throw new NotImplementedException();
    }
    public void MakeShoot()
    {
        ClientTCP.PACKAGE_Match_Shot(matchID);
    }







}
