using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public Queue<UICard> animationQueue;


    public ClientMatchManager MatchManager;

    public Sequence notSelectedCardSequence;
    public Sequence comboCardsSequences;
    public Sequence noComboCardsSequence;
    public Sequence enemyCardsSequence;
    public Sequence wholeSequence;

    public bool PlayTimerState;
    public float TimeRemains;
    public float TimerTime;

    public UICard[] UICards;
    public UICard[] enemyUICards;

    public Text winnerText;




    public void Start()
    {
        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.AutoPlaySequences;
        MatchManager = GetComponent<ClientMatchManager>();
    }

    public void StartTimer(float time)
    {
        throw new NotImplementedException();
    }

    public void ShowCards()
    {

        ShowShotButton();
        //TODO: Rework architecture with CARDS, Change to ArrayList?
        wholeSequence = DOTween.Sequence();
        for (int i = 0; i < UICards.Length; i++)
        {
            var uiCard = UICards[i];
            var card = MatchManager.Cards[i];
            uiCard.cardImage.sprite = ClientManager.allCardsSprites[card.ID];
            uiCard.directionImage.sprite = ClientManager.directionSprites[card.direction];
            uiCard.direction = card.direction;
            uiCard.cardValue.text = card.Value.ToString();
            uiCard.PlayShowCard();
            wholeSequence.Join(uiCard.animationSequence);

        }
        wholeSequence.Play();
        
    }

    public void ToggleCard(int position)
    {
        throw new NotImplementedException();
    }

    public void MakeShoot()
    {
        throw new NotImplementedException();
    }

    public void ShowResult()
    {

        HideShotButton();

        HideCards();

        wholeSequence = DOTween.Sequence();

        if (MatchManager.amIShot)
        {
            MatchManager.PlayerEntityController.Shot();
            PlayPlayerCards();
            PlayEnemyCards();
        }
        else
        {
            MatchManager.EnemyEntityController.Shot();
            PlayEnemyCards();
            PlayPlayerCards();
        }

      


        //To ensure that UI Displays correct values
        wholeSequence.AppendCallback(() =>
       {

          
           if (MatchManager.PlayerEntityController.HP != MatchManager.PlayerHP)
           {
               Debug.LogError("Player has Incorrect HP Value");
               Debug.LogFormat("Has {0}, but should has {1}", MatchManager.PlayerEntityController.HP, MatchManager.PlayerHP);
               MatchManager.PlayerEntityController.HP = MatchManager.PlayerHP;
               MatchManager.PlayerEntityController.UpdateUI();
           }

           if (MatchManager.PlayerEntityController.Armor != MatchManager.PlayerArmor)
           {
               Debug.LogError("Player has Incorrect Armor Value");
               Debug.LogFormat("Has {0}, but should has {1}", MatchManager.PlayerEntityController.Armor, MatchManager.PlayerArmor);
               MatchManager.PlayerEntityController.Armor = MatchManager.PlayerArmor;
               MatchManager.PlayerEntityController.UpdateUI();
           }

           if (MatchManager.EnemyEntityController.HP != MatchManager.EnemyHP)
           {
               Debug.LogError("Enemy has Incorrect HP Value");
               Debug.LogFormat("Has {0}, but should has {1}", MatchManager.EnemyEntityController.HP, MatchManager.EnemyHP);
               MatchManager.EnemyEntityController.HP = MatchManager.EnemyHP;
               MatchManager.EnemyEntityController.UpdateUI();
           }

           if (MatchManager.EnemyEntityController.Armor != MatchManager.EnemyArmor)
           {
               Debug.LogError("Enemy has Incorrect Armor Value");
               Debug.LogFormat("Has {0}, but should has {1}", MatchManager.EnemyEntityController.Armor, MatchManager.EnemyArmor);
               MatchManager.EnemyEntityController.Armor = MatchManager.EnemyArmor;
               MatchManager.EnemyEntityController.UpdateUI();
           }
       });

        wholeSequence.AppendCallback(MatchManager.SendSetReady);
        wholeSequence.Play();
    }

    public void HideShotButton()
    {
        MatchManager.shootButton.gameObject.SetActive(false);
    }

    public void ShowShotButton()
    {
        MatchManager.shootButton.gameObject.SetActive(true);
    }

    public void FinishGame(string winnerUsername)
    {
        if (winnerUsername == "Draw")
        {
            MatchManager.PlayerEntityController.Die();
            MatchManager.EnemyEntityController.Die();
        }
        else
        {
            var looser = MatchManager.PlayerEntityController.userName == winnerUsername
                ? MatchManager.EnemyEntityController
                : MatchManager.PlayerEntityController;
            looser.Die();
        }

        winnerText.enabled = true;
        winnerText.text = "The winner is: \n" + winnerUsername + "!"; //+\n Press RMB for Restart";

    }


    private void HideCards()
    {
        var kostil = "kostil";
        notSelectedCardSequence = DOTween.Sequence();
        foreach (var cardPos in MatchManager.PlayerNotSelectedCards)
        {
            UICards[cardPos].PlayAnimation(CardAnimationsStates.NotSelectedCard);
            notSelectedCardSequence.Join(UICards[cardPos].animationSequence);
        }

        notSelectedCardSequence.AppendCallback((() => kostil = "kostil"));
        wholeSequence.Append(notSelectedCardSequence);
    }

    private void PlayPlayerCards()
    {
        var kostil = "kostil";
       
        noComboCardsSequence = DOTween.Sequence();
        foreach (var cardPos in MatchManager.PlayerSoloCards)
        {
            var card = MatchManager.Cards[cardPos];


            UICards[cardPos].effectSprite = ClientManager.allEffectsSprites[card.ID];
            UICards[cardPos].PlayAnimation(CardAnimationsStates.NoCombo);
            noComboCardsSequence.Append(UICards[cardPos].animationSequence);

            var cowboy = card.direction == 0 ? MatchManager.PlayerEntityController : MatchManager.EnemyEntityController;
            noComboCardsSequence.AppendCallback(() => cowboy.HitByCard(card));
        }
        noComboCardsSequence.AppendCallback((() => kostil = "kostil"));
        wholeSequence.Append(noComboCardsSequence);




        comboCardsSequences = DOTween.Sequence();
        for (var i = 0; i < MatchManager.PlayerComboCards.Count; i++)
        {
            Sequence tempSequence = DOTween.Sequence();
            var combination = MatchManager.PlayerComboCards[i];
            var comboSprite = ClientManager.allCardsSprites[combination[0]];
            var effectSprite = ClientManager.allEffectsSprites[combination[0]];
            var direction = combination[1];

            for (var j = 2; j < combination.Count; j++)
            {
                var cardPos = combination[j];
               
                UICards[cardPos].comboCardSprite = comboSprite;
                UICards[cardPos].effectSprite = effectSprite;
                UICards[cardPos].PlayAnimation(CardAnimationsStates.Combo);

                tempSequence.Join(UICards[cardPos].animationSequence);
            }

            comboCardsSequences.Append(tempSequence);
            var cowboy = direction == 0 ? MatchManager.PlayerEntityController : MatchManager.EnemyEntityController;
            var card = ClientManager.allCardsInfo[combination[0]]; //combination 0 - Result Combo Card ID
            comboCardsSequences.AppendCallback(() => cowboy.HitByCard(card));
        }
        comboCardsSequences.AppendCallback((() => kostil = "kostil"));
        wholeSequence.Append(comboCardsSequences);
    }


    private void PlayEnemyCards()
    {
        string kostil = "kostil";
        enemyCardsSequence = DOTween.Sequence();
        for (int i = 0; i < MatchManager.EnemySelectedCards.Count; i = i + 2)
        {
            var cardID = MatchManager.EnemySelectedCards[i];
            var direction = MatchManager.EnemySelectedCards[i + 1];
            direction = direction == 0 ? 1 : 0; //because it's direction for enemy, not for use. 0 - means that enemy shot himself, but for us it's 1
            
            var cardData = ClientManager.allCardsInfo[cardID];
            var cardPos = i / 2;
            
            //Enemy Card is placed on 5-th position of the array
            enemyUICards[cardPos].cardSprite = ClientManager.allCardsSprites[cardID];
            enemyUICards[cardPos].directionImage.sprite = ClientManager.directionSprites[direction];
            enemyUICards[cardPos].direction = direction;
            enemyUICards[cardPos].cardValue.text = cardData.Value.ToString();
            enemyUICards[cardPos].effectSprite = ClientManager.allEffectsSprites[cardID];

            enemyUICards[cardPos].PlayAnimation(CardAnimationsStates.EnemyCard);

            enemyCardsSequence.Append(enemyUICards[cardPos].animationSequence);
            var cowboy = direction == 0 ? MatchManager.PlayerEntityController : MatchManager.EnemyEntityController;
            enemyCardsSequence.AppendCallback(() => cowboy.HitByCard(cardData));
            
        }
        enemyCardsSequence.AppendCallback((() => kostil = "kostil"));
        wholeSequence.Append(enemyCardsSequence);
    }

    public static class CardAnimationsStates
    {
        public const string Combo = "Combo";
        public const string NoCombo = "NoCombo";
        public const string EnemyCard = "EnemyCard";
        public const string NotSelectedCard = "NotSelectedCard";
    }
}