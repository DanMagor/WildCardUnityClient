using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class AnimationManager : MonoBehaviour
{
    public ClientMatchManager MatchManager;
    public Text PlayerHPLabel;
    public Text PlayerArmorLabel;
    public Text EnemyHPLabel;
    public Text EnemyArmorLabel;
    public bool PlayTimerState;
    public float TimerTime;
    public float TimeRemains;
    public UICard[] UICards;
    public bool inAnimation;
    public Queue<UICard> animationQueue;

    public Sequence animationSequence;

    public static class CardAnimationsStates
    {
        public const string Combo = "Combo";
        public const string NoCombo = "NoCombo";

    }

   
    


    private void Awake()
    {
      UICards = GetComponentsInChildren<UICard>();
      animationSequence = DOTween.Sequence();
      MatchManager = GetComponent<ClientMatchManager>();
    }

    public void StartTimer(float time)
    {
        throw new NotImplementedException();
    }

    public void ShowCards()
    {
        //TODO: Rework architecture with CARDS, Change to ArrayList?
        var i = 0;
        foreach (var uiCard in UICards)
        {
            var cardID = MatchManager.Cards[i].ID;
            uiCard.cardImage.sprite = ClientManager.allCardsSprites[cardID];
            i++;
        }
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

        animationSequence.Kill();
        animationSequence = DOTween.Sequence();


        foreach (var cardPos in MatchManager.PlayerNotSelectedCards)
        {
            UICards[cardPos].PlayGoOff();
        }

        foreach (var card in MatchManager.PlayerSoloCards)
        {
            UICards[card].PlayAnimation(CardAnimationsStates.NoCombo);
        }

        foreach (var combinations in MatchManager.PlayerComboCards)
        {
            int i = 0;
            Sprite comboSprite = null;
            int direction = 0;
            foreach (var cardPos in combinations)
            {
                if (i == 0)
                {
                    comboSprite = ClientManager.allCardsSprites[cardPos];
                }

                if (i == 1)
                {
                    direction = cardPos;
                }

                UICards[cardPos].comboCardSprite = comboSprite;
                UICards[cardPos].PlayAnimation(CardAnimationsStates.Combo);
            }
        }


        

       
        
       
        animationSequence.AppendCallback(MatchManager.SendSetReady);
        animationSequence.Play();


    }


}
    