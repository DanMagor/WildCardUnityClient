using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private MatchManager matchManager;

    #region Animation Sequences
    private Sequence notSelectedCardSequence;
    private Sequence comboCardsSequences;
    private Sequence noComboCardsSequence;
    private Sequence enemyCardsSequence;
    private Sequence wholeSequence;
    #endregion

    
    #region Cards and Timer UI
    [SerializeField]
    private UICard[] UICards;

    [SerializeField]
    private UICard[] enemyUICards;
    #endregion

    [SerializeField]
    private Timer timer; 

    //Todo Change to logic with animation
    #region Text UI
    [SerializeField]
    private Text nAttackCardsCounter;
    [SerializeField]
    private Text nHealCardsCounter;
    [SerializeField]
    private Text nArmorCardsCounter;
    [SerializeField]
    private Text winnerText;
    #endregion


    public void Start()
    {
        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.AutoPlaySequences;
        matchManager = GetComponent<MatchManager>();
    }
   
    public void StartTimer(float time)
    {
        timer.totalWaitingTime = time;       
        timer.enabled = true;
    }

    public void ShowCards()
    {
       

        ShowShotButton();
        //TODO: Rework architecture with CARDS, Change to ArrayList?
        wholeSequence = DOTween.Sequence();
        for (int i = 0; i < UICards.Length; i++)
        {
            var uiCard = UICards[i];
            var card = matchManager.Cards[i];
            uiCard.Selected = false; //set default value for selection

            uiCard.CardImage.sprite = ClientManager.AllCardsSprites[card.Id];

            uiCard.direction = card.Direction;
            uiCard.DirectionImage.sprite = ClientManager.DirectionSprites[card.Direction];
          
            uiCard.CardValue.text = card.Value.ToString();
            uiCard.PlayShowCard();
            wholeSequence.Join(uiCard.AnimationSequence);

        }
        nAttackCardsCounter.text = matchManager.nAttackCardsInDeck.ToString();
        nHealCardsCounter.text = matchManager.nHealCardsInDeck.ToString();
        nArmorCardsCounter.text = matchManager.nArmorCardsInDeck.ToString();


        wholeSequence.Play();
        
    }

    public void ShowResult()
    {

        wholeSequence = DOTween.Sequence();

        HideShotButton();

        HideCards();

      

        if (matchManager.amIShot)
        {
            matchManager.PlayerEntityController.Shot();
            PlayPlayerCards();
            PlayEnemyCards();
        }
        else
        {
            matchManager.EnemyEntityController.Shot();
            PlayEnemyCards();
            PlayPlayerCards();
        }

      


        //To ensure that UI Displays correct values
        wholeSequence.AppendCallback(() =>
       {

          
           if (matchManager.PlayerEntityController.HP != matchManager.PlayerHP)
           {
               Debug.LogError("Player has Incorrect HP Value");
               Debug.LogFormat("Has {0}, but should has {1}", matchManager.PlayerEntityController.HP, matchManager.PlayerHP);
               matchManager.PlayerEntityController.HP = matchManager.PlayerHP;
               matchManager.PlayerEntityController.UpdateUI();
           }

           if (matchManager.PlayerEntityController.Armor != matchManager.PlayerArmor)
           {
               Debug.LogError("Player has Incorrect Armor Value");
               Debug.LogFormat("Has {0}, but should has {1}", matchManager.PlayerEntityController.Armor, matchManager.PlayerArmor);
               matchManager.PlayerEntityController.Armor = matchManager.PlayerArmor;
               matchManager.PlayerEntityController.UpdateUI();
           }

           if (matchManager.EnemyEntityController.HP != matchManager.EnemyHP)
           {
               Debug.LogError("Enemy has Incorrect HP Value");
               Debug.LogFormat("Has {0}, but should has {1}", matchManager.EnemyEntityController.HP, matchManager.EnemyHP);
               matchManager.EnemyEntityController.HP = matchManager.EnemyHP;
               matchManager.EnemyEntityController.UpdateUI();
           }

           if (matchManager.EnemyEntityController.Armor != matchManager.EnemyArmor)
           {
               Debug.LogError("Enemy has Incorrect Armor Value");
               Debug.LogFormat("Has {0}, but should has {1}", matchManager.EnemyEntityController.Armor, matchManager.EnemyArmor);
               matchManager.EnemyEntityController.Armor = matchManager.EnemyArmor;
               matchManager.EnemyEntityController.UpdateUI();
           }
       });

        wholeSequence.AppendCallback(matchManager.SendSetReady);
        wholeSequence.Play();
    }

    public void ToggleCard(int position)
    {
        UICards[position].ToggleCardSelection();
    }

    public void MakeShot()
    {
        throw new NotImplementedException();
    }

    
    public void HideShotButton()
    {
        matchManager.shotButton.gameObject.SetActive(false);
    }

    public void ShowShotButton()
    {
        matchManager.shotButton.gameObject.SetActive(true);
    }

    public void FinishGame(string winnerUsername)
    {
        if (winnerUsername == "Draw")
        {
            matchManager.PlayerEntityController.Die();
            matchManager.EnemyEntityController.Die();
        }
        else
        {
            var looser = matchManager.PlayerEntityController.Username == winnerUsername
                ? matchManager.EnemyEntityController
                : matchManager.PlayerEntityController;
            looser.Die();
        }

        winnerText.enabled = true;
        winnerText.text = "The winner is: \n" + winnerUsername + "!"; //+\n Press RMB for Restart";

    }


    public void HideCards()
    {
        var kostil = "kostil";
        notSelectedCardSequence = DOTween.Sequence();
        foreach (var cardPos in matchManager.PlayerNotSelectedCards)
        {
            UICards[cardPos].PlayAnimation(CardAnimationsStates.NotSelectedCard);
            notSelectedCardSequence.Join(UICards[cardPos].AnimationSequence);
        }

        //Todo Change to Logic with Animation
        nAttackCardsCounter.text = matchManager.nAttackCardsInDeck.ToString();
        nHealCardsCounter.text = matchManager.nHealCardsInDeck.ToString();
        nArmorCardsCounter.text = matchManager.nArmorCardsInDeck.ToString();



        notSelectedCardSequence.AppendCallback((() => kostil = "kostil"));
        wholeSequence.Append(notSelectedCardSequence);
    }

    private void PlayPlayerCards()
    {
        var kostil = "kostil";
       
        noComboCardsSequence = DOTween.Sequence();
        foreach (var cardPos in matchManager.PlayerSoloCards)
        {
            var card = matchManager.Cards[cardPos];


            UICards[cardPos].ItemSprite = ClientManager.AllItemsSprites[card.Id];
            UICards[cardPos].PlayAnimation(CardAnimationsStates.NoCombo);
            noComboCardsSequence.Append(UICards[cardPos].AnimationSequence);

            var cowboy = card.Direction == 0 ? matchManager.PlayerEntityController : matchManager.EnemyEntityController;
            noComboCardsSequence.AppendCallback(() => cowboy.HitByCard(card));
        }
        noComboCardsSequence.AppendCallback((() => kostil = "kostil"));
        wholeSequence.Append(noComboCardsSequence);




        comboCardsSequences = DOTween.Sequence();
        for (var i = 0; i < matchManager.PlayerComboCards.Count; i++)
        {
            Sequence tempSequence = DOTween.Sequence();
            var combination = matchManager.PlayerComboCards[i];
            var comboSprite = ClientManager.AllCardsSprites[combination[0]];
            var effectSprite = ClientManager.AllItemsSprites[combination[0]];
            var direction = combination[1];

            for (var j = 2; j < combination.Count; j++)
            {
                var cardPos = combination[j];
               
                UICards[cardPos].ComboCardSprite = comboSprite;
                UICards[cardPos].ItemSprite = effectSprite;
                UICards[cardPos].PlayAnimation(CardAnimationsStates.Combo);

                tempSequence.Join(UICards[cardPos].AnimationSequence);
            }

            comboCardsSequences.Append(tempSequence);
            var cowboy = direction == 0 ? matchManager.PlayerEntityController : matchManager.EnemyEntityController;
            var card = ClientManager.AllCardsInfo[combination[0]]; //combination 0 - Result Combo Card ID
            comboCardsSequences.AppendCallback(() => cowboy.HitByCard(card));
        }
        comboCardsSequences.AppendCallback((() => kostil = "kostil"));
        wholeSequence.Append(comboCardsSequences);
    }


    private void PlayEnemyCards()
    {
        string kostil = "kostil";
        enemyCardsSequence = DOTween.Sequence();
        for (int i = 0; i < matchManager.EnemySelectedCards.Count; i = i + 2)
        {
            var cardID = matchManager.EnemySelectedCards[i];
            var direction = matchManager.EnemySelectedCards[i + 1];
            direction = direction == 0 ? 1 : 0; //because it's direction for enemy, not for use. 0 - means that enemy shot himself, but for us it's 1
            
            var cardData = ClientManager.AllCardsInfo[cardID];
            var cardPos = i / 2;
            
            //Enemy Card is placed on 5-th position of the array
            enemyUICards[cardPos].CardSprite = ClientManager.AllCardsSprites[cardID];
            enemyUICards[cardPos].DirectionImage.sprite = ClientManager.DirectionSprites[direction];
            enemyUICards[cardPos].direction = direction;
            enemyUICards[cardPos].CardValue.text = cardData.Value.ToString();
            enemyUICards[cardPos].ItemSprite = ClientManager.AllItemsSprites[cardID];

            enemyUICards[cardPos].PlayAnimation(CardAnimationsStates.EnemyCard);

            enemyCardsSequence.Append(enemyUICards[cardPos].AnimationSequence);
            var cowboy = direction == 0 ? matchManager.PlayerEntityController : matchManager.EnemyEntityController;
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