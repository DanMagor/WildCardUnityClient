﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICard : MonoBehaviour, IPointerClickHandler
{
    public static float movingTime = 0.3f;
    public static float transformationTime = 0.5f;


    private Vector3 m_StartingAnchorPosition;
    public static Vector3 scaleRate = new Vector3(0.3f, 0.3f);


    private AnimationManager animationManager;



    public Image cardImage;

    //TODO: Temp Remove
    public GameObject player;
    //

    public Animator CardUIAnimator;
    public RectTransform firstPoint;
    public MatchInputManager InputManager;

    public ParticleSystem noComboParticleSystem;

    public int Position;
    private RectTransform rTransform;

    public Sprite effectSprite;
    public Sprite cardSprite;
    public Sprite comboCardSprite;

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO: Check in InputManager that it's possible to click on Card
        if (animationManager.animationSequence.active)
        {

            return;
        }

        if (animationManager.animationSequence.active)
        {
            animationManager.animationSequence.Pause();
        }
        InputManager.ToggleCard(Position);
    }

    private void Awake()
    {
        InputManager = GetComponentInParent<MatchInputManager>();
        CardUIAnimator = GetComponentInParent<Animator>();
        rTransform = GetComponent<RectTransform>();
        cardImage = GetComponent<Image>();
        animationManager = GetComponentInParent<AnimationManager>();
        m_StartingAnchorPosition = rTransform.anchoredPosition;

    }

    private void Start()
    {
    }

    private void Update()
    {
    }


    public void PlayAnimation(string animationName)
    {

        switch (animationName)
        {
            case AnimationManager.CardAnimationsStates.Combo:
                PlayCombo();
                break;
            case AnimationManager.CardAnimationsStates.NoCombo:
                PlayNoCombo();
                break;
        }




    }


    private void PlayCombo()
    {
        Sequence animationSequence = DOTween.Sequence();
        CardUIAnimator.enabled = false;
        firstPoint.gameObject.SetActive(false);

        var mainParticleSys = noComboParticleSystem.main;
        if (noComboParticleSystem.isStopped)
        {
            mainParticleSys.duration = transformationTime;
        }

        animationSequence.Append(
            rTransform.DOAnchorPos(firstPoint.anchoredPosition, movingTime));
        animationSequence.Join(
            rTransform.DOScale(scaleRate, movingTime));
        animationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Play();
            
        }));
       

        var newColor = cardImage.color;
        newColor.a = 0f;

        animationSequence.Append(cardImage.DOColor(newColor, transformationTime / 2));

        newColor = cardImage.color;
        newColor.a = 1f;
        animationSequence.AppendCallback((() =>
        {
            cardImage.sprite = comboCardSprite;
        }));
        animationSequence.Append(cardImage.DOColor(newColor, transformationTime / 2));

        animationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Stop();
            noComboParticleSystem.Clear();
        }));

        animationSequence.Join(rTransform.DOScale(new Vector3(1f, 1f), transformationTime / 2));
        animationSequence.AppendInterval(transformationTime / 3);
        animationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Play();
        }));

        rTransform.localScale = scaleRate;
        newColor.a = 0f;
        animationSequence.Append(cardImage.DOColor(newColor, transformationTime / 2));
        animationSequence.Join(rTransform.DOScale(scaleRate, transformationTime / 2));

        newColor = cardImage.color;
        newColor.a = 1f;
        animationSequence.AppendCallback((() =>
        {
            cardImage.sprite = effectSprite;
        }));
        animationSequence.Append(cardImage.DOColor(newColor, transformationTime / 2));
        animationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Stop();
        }));
            


        var position = Camera.main.WorldToScreenPoint(player.transform.position);
        animationSequence.Append(rTransform.DOMove(position, movingTime));
        animationSequence.AppendCallback(() =>
        {
            rTransform.anchoredPosition = m_StartingAnchorPosition;
            cardImage.sprite = cardSprite;
            rTransform.localScale = new Vector3(1f, 1f);
        });

        animationManager.animationSequence.Join(animationSequence);

    }

    private void PlayNoCombo()
    {
        var animationSequence = DOTween.Sequence();
        CardUIAnimator.enabled = false;
        firstPoint.gameObject.SetActive(false);

        var mainParticleSys = noComboParticleSystem.main;
        if (noComboParticleSystem.isStopped)
        {
            mainParticleSys.duration = transformationTime;
        }

        animationSequence.Append(
            rTransform.DOAnchorPos(firstPoint.anchoredPosition, movingTime));
        animationSequence.Join(
            rTransform.DOScale(scaleRate, movingTime));
        animationSequence.AppendCallback(() => noComboParticleSystem.Play());
        

        var newColor = cardImage.color;
        newColor.a = 0f;

        animationSequence.Append(cardImage.DOColor(newColor, transformationTime / 2));


        newColor = cardImage.color;
        newColor.a = 1f;

        animationSequence.AppendCallback((() =>
        {
            cardImage.sprite = effectSprite;
        }));
        animationSequence.Append(cardImage.DOColor(newColor, transformationTime / 2));
        animationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Stop(); 

        }));

        var position = Camera.main.WorldToScreenPoint(player.transform.position);
        animationSequence.Append(rTransform.DOMove(position, movingTime));
        animationSequence.AppendCallback(() =>
        {
            rTransform.anchoredPosition = m_StartingAnchorPosition;
            cardImage.sprite = cardSprite;
            rTransform.localScale = new Vector3(1f, 1f);
        });

        animationManager.animationSequence.Append(animationSequence);

    }

    public void PlayGoOff()
    {
        Debug.Log("Go Off");
    }
}