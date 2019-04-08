﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Assets.Prefabs.DataSaving;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; //TODO: Delete

public class ClientMatchManager : MonoBehaviour
{


    #region RoundResults

    [NonSerialized]
    public bool amIShot;
    [NonSerialized]
    public List<int> PlayerSoloCards;
    [NonSerialized]
    public List<List<int>> PlayerComboCards;
    [NonSerialized]
    public List<int> PlayerNotSelectedCards;
    [NonSerialized]
    public List<int> EnemySelectedCards;
  
    

    



    #endregion

   

    public CardInstanceSerializable[] Cards;

    public int matchID;

    public AnimationManager AnimationManager;
    public CowboyEntityController PlayerEntityController;
    public CowboyEntityController EnemyEntityController;

    public ShootButton shootButton;

    public static MatchStates matchState;
    public enum MatchStates
    {
        CardChoosing,
        ResultShowing,
        Waiting
    }

    private void Awake()
    {
        AnimationManager = GetComponent<AnimationManager>();
        matchState = MatchStates.Waiting;

    }

    private void Start()
    {
        
    }

    public void ShowResult(ByteBuffer buffer)
    {
        if(matchState != MatchStates.CardChoosing && matchState != MatchStates.Waiting) return;
        matchState = MatchStates.ResultShowing;

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
            int length2 = buffer.ReadInteger();
           
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
        


        var PlayerHP = buffer.ReadInteger();
        var PlayerArmor = buffer.ReadInteger();

        

        EnemySelectedCards = new List<int>();
        length = buffer.ReadInteger();
        for (int i = 0; i < length; i++)
        {
            EnemySelectedCards.Add(buffer.ReadInteger()); //Id then Direction
        }

        var EnemyHP = buffer.ReadInteger();
        var EnemyArmor = buffer.ReadInteger();



   
        AnimationManager.ShowResult();


        //To ensure that UI Displays correct values
        PlayerEntityController.HP = PlayerHP;
        PlayerEntityController.Armor = PlayerArmor;
        PlayerEntityController.UpdateUI();

        EnemyEntityController.HP = EnemyHP;
        EnemyEntityController.Armor = EnemyArmor;
        EnemyEntityController.UpdateUI();



    }

    

    public void SendSetReady()
    {
       matchState = MatchStates.Waiting;
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
        if (matchState != MatchStates.Waiting) return;
        matchState = MatchStates.CardChoosing;
        AnimationManager.ShowCards();
    }

   

    public void ToggleCard(int position)
    {
        ClientTCP.PACKAGE_Match_ToggleCard(matchID, position);
    }
    public void MakeShoot()
    {
        matchState = MatchStates.Waiting;
        ClientTCP.PACKAGE_Match_Shot(matchID);
    }

    public void FinishGame(ByteBuffer data)
    {
        var winnerUsername = data.ReadString();
        AnimationManager.FinishGame(winnerUsername);
        
    }

    public void RestartMatch()
    {
        ClientTCP.PACKAGE_SendRestartMatch(matchID);
    }





}
