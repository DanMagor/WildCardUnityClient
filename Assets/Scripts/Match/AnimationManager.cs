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
    }

    public void StartTimer(float time)
    {
        throw new NotImplementedException();
    }

    public void ShowCards()
    {
        throw new NotImplementedException();
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
        //temp:
        int cardPosition = 1;
        animationSequence = DOTween.Sequence();
       
        UICards[0].PlayAnimation(CardAnimationsStates.Combo);
        UICards[1].PlayAnimation(CardAnimationsStates.Combo);
        UICards[2].PlayAnimation(CardAnimationsStates.NoCombo);
        UICards[3].PlayAnimation(CardAnimationsStates.NoCombo);

        
    }


}
    