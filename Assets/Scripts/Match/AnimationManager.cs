using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public Queue<UICard> animationQueue;
    
    public Text EnemyArmorLabel;
    public Text EnemyHPLabel;
    public bool inAnimation;
    public ClientMatchManager MatchManager;
    public Sequence comboCardsSequences;
    public Sequence noComboCardsSequence;
    public Text PlayerArmorLabel;
    public Text PlayerHPLabel;
    public bool PlayTimerState;
    public float TimeRemains;
    public float TimerTime;
    public UICard[] UICards;

    public Sequence wholeSequence;


    private void Awake()
    {
        UICards = GetComponentsInChildren<UICard>();
        wholeSequence = DOTween.Sequence();
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
        noComboCardsSequence = DOTween.Sequence();
        comboCardsSequences = DOTween.Sequence();
        wholeSequence = DOTween.Sequence();


        foreach (var cardPos in MatchManager.PlayerNotSelectedCards)
        {
            Debug.LogFormat("Card {0} goes off", cardPos);
            UICards[cardPos].PlayGoOff();
        }

        foreach (var cardPos in MatchManager.PlayerSoloCards)
        {
            Debug.LogFormat("Card {0} NoCombo", cardPos);
            UICards[cardPos].PlayAnimation(CardAnimationsStates.NoCombo);
            noComboCardsSequence.Append(UICards[cardPos].animationSequence);
        }

        wholeSequence.Append(noComboCardsSequence);


        for (var i = 0; i < MatchManager.PlayerComboCards.Count; i++)
        {
            Sequence tempSequence = DOTween.Sequence();
            var combination = MatchManager.PlayerComboCards[i];
            var comboSprite = ClientManager.allCardsSprites[combination[0]];
            var direction = combination[1];

            for (var j = 2; j < combination.Count; j++)
            {
                var cardPos = combination[j];
                Debug.LogFormat("Card {0} in Combo", cardPos);
                UICards[cardPos].comboCardSprite = comboSprite;
                UICards[cardPos].PlayAnimation(CardAnimationsStates.Combo);

                tempSequence.Join(UICards[cardPos].animationSequence);
            }

            comboCardsSequences.Append(tempSequence);

        }
        wholeSequence.Append(comboCardsSequences);

        wholeSequence.AppendCallback(MatchManager.SendSetReady);
        //wholeSequence.Play();
    }

    public static class CardAnimationsStates
    {
        public const string Combo = "Combo";
        public const string NoCombo = "NoCombo";
    }
}