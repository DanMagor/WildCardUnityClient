﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Assets.Prefabs.DataSaving;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI; //TODO: Delete

public class MatchManager : MonoBehaviour
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


    [NonSerialized]
    public int PlayerHP;
    [NonSerialized]
    public int PlayerArmor;

    [NonSerialized]
    public int EnemyHP;
    [NonSerialized]
    public int EnemyArmor;


    [NonSerialized]
    public int nAttackCardsInDeck;
    [NonSerialized]
    public int nHealCardsInDeck;
    [NonSerialized]
    public int nArmorCardsInDeck;


    #endregion

    public CardEntity[] Cards;

    public int MatchId;

    [SerializeField]
    private AnimationManager animationManager;
    public CowboyEntityController PlayerEntityController;
    public CowboyEntityController EnemyEntityController;

    public ShotButton shotButton;

    public static MatchStates matchState;
    public enum MatchStates
    {
        CardChoosing,
        ResultShowing,
        Waiting
    }



    private void Start()
    {
        animationManager = GetComponent<AnimationManager>();
        matchState = MatchStates.Waiting;
    }
    public void StartRound(ByteBuffer data)
    {
        data.ReadInteger();//Read Package ID
        StartTimer(data.ReadFloat());
    }
    private void StartTimer(float timerTime)
    {
        animationManager.StartTimer(timerTime);
    }
    public void ShowCards(ByteBuffer data)
    {
        if (matchState != MatchStates.Waiting) return;
        matchState = MatchStates.CardChoosing;
        animationManager.ShowCards();
    }

    public void HandleSendedCards(ByteBuffer data)
    {

        data.ReadInteger(); //Read package ID

        var numberOfCards = data.ReadInteger();


        Cards = new CardEntity[numberOfCards];

        for (var i = 0; i < numberOfCards; i++)
        {
            Cards[i] = ClientManager.AllCardsInfo[data.ReadInteger()].Clone();
            Cards[i].Direction = data.ReadInteger();
        }

        nAttackCardsInDeck = data.ReadInteger();
        nHealCardsInDeck = data.ReadInteger();
        nArmorCardsInDeck = data.ReadInteger();
    }
    public void ShowResult(ByteBuffer buffer)
    {
        if (matchState != MatchStates.CardChoosing && matchState != MatchStates.Waiting) return;
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



        PlayerHP = buffer.ReadInteger();
        PlayerArmor = buffer.ReadInteger();



        EnemySelectedCards = new List<int>();
        length = buffer.ReadInteger();
        for (int i = 0; i < length; i++)
        {
            EnemySelectedCards.Add(buffer.ReadInteger()); //Id then direction
        }

        EnemyHP = buffer.ReadInteger();
        EnemyArmor = buffer.ReadInteger();

        nAttackCardsInDeck = buffer.ReadInteger();
        nHealCardsInDeck = buffer.ReadInteger();
        nArmorCardsInDeck = buffer.ReadInteger();


        animationManager.ShowResult();






    }
    public void SendSetReady()
    {
        matchState = MatchStates.Waiting;
        ClientTCP.PACKAGE_Match_SetReady(MatchId);
    }
    public void SendToggleCard(int position)
    {
        ClientTCP.PACKAGE_Match_ToggleCard(MatchId, position);
    }
    public void ConfirmToggleCard(int cardPos)
    {
        animationManager.ToggleCard(cardPos);
    }
    public void MakeShot()
    {
        matchState = MatchStates.Waiting;
        ClientTCP.PACKAGE_Match_Shot(MatchId);
    }


    public void FinishGame(ByteBuffer data)
    {
        var winnerUsername = data.ReadString();
        animationManager.FinishGame(winnerUsername);

    }
    public void RestartMatch()
    {
        ClientTCP.PACKAGE_Match_RequestRestartMatch(MatchId);
    }

    




}