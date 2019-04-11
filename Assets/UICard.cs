using DG.Tweening;
using System;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICard : MonoBehaviour, IPointerClickHandler
{
    public static float movingTime = 0.5f; //0.3f
    public static float transformationTime = 0.5f;
    public static float hidingShowingTime = 0.1f;


    private Vector3 m_StartingAnchorPosition;
    private Color m_InitialColor;
    private Vector3 m_InitialScaleRate;

    private Vector3 m_HiddenPosition;
    private Vector3 m_ShownPosition;

    public static Vector3 scaleRate = new Vector3(0.3f, 0.3f);


    private AnimationManager animationManager;

    public Sequence animationSequence;

    public Image cardImage;
    public Image directionImage;
    public Text cardValue;


    public GameObject player;
    public GameObject enemy;
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

    [NonSerialized]
    public bool selected = false;


    public int direction;

    public void OnPointerClick(PointerEventData eventData)
    {

        InputManager.ToggleCard(Position);
    }

    private void Awake()
    {
        InputManager = GetComponentInParent<MatchInputManager>();
        CardUIAnimator = GetComponentInParent<Animator>();
        rTransform = GetComponent<RectTransform>();
        cardImage = GetComponent<Image>();
        cardValue = GetComponentInChildren<Text>();

        animationManager = GetComponentInParent<AnimationManager>();
        m_StartingAnchorPosition = rTransform.anchoredPosition;
        m_InitialColor = cardImage.color;
        m_InitialScaleRate = rTransform.localScale;

        m_HiddenPosition = m_StartingAnchorPosition;
        m_ShownPosition = m_StartingAnchorPosition + new Vector3(0, +rTransform.rect.height, 0);


    }


 



    public void PlayAnimation(string animationName)
    {
        DOTween.defaultAutoPlay = AutoPlay.AutoPlaySequences;
        animationSequence = DOTween.Sequence();
        //TODO: Delete switch from here and Animation Manager, change to methods
        switch (animationName)
        {
            case AnimationManager.CardAnimationsStates.Combo:
                PlayComboAnimation();
                break;
            case AnimationManager.CardAnimationsStates.NoCombo:
                PlayNoComboAnimation();
                break;
            case AnimationManager.CardAnimationsStates.EnemyCard:
                PlayEnemyCardAnimation();
                break;
            case AnimationManager.CardAnimationsStates.NotSelectedCard:
                PlayNotSelectedCard();
                break;
        }


        var newColor = directionImage.color;
        newColor.a = 1f;
        //TODO: IMPORTANT! maybe delete later //Setup default Colors for Cards
        animationSequence.Append(cardValue.DOColor(newColor, 0));
        animationSequence.Append(directionImage.DOColor(newColor, 0));


    }


    public void PlayNotSelectedCard()
    {
       
        animationSequence.Append(rTransform.DOAnchorPos(m_HiddenPosition, hidingShowingTime));
    }

    public void PlayShowCard()
    {
        animationSequence = DOTween.Sequence();
        animationSequence.Append(rTransform.DOAnchorPos(m_ShownPosition, hidingShowingTime));
    }

    public void PlayEnemyCardAnimation()
    {
       
        MoveToCenterEnemyCard();
        PlayNoComboEffect();
        PlaceEnemyCardToInitialPosition();

    }

    private void MoveToCenterEnemyCard()
    {
        CardUIAnimator.enabled = false;
       


        cardImage.sprite = cardSprite;


        animationSequence.Append(
            rTransform.DOAnchorPos(firstPoint.anchoredPosition, movingTime/2));
        animationSequence.Join(
            rTransform.DOScale(new Vector3(1f, 1f, 1f), movingTime/2));
        var newColor = m_InitialColor;
        newColor.a = 1;
        var textColor = cardValue.color;
        textColor.a = 1;
        animationSequence.Join(cardValue.DOColor(textColor, movingTime / 2));
        animationSequence.Join(cardImage.DOColor(newColor, movingTime / 2));
           //  animationSequence.Join(directionImage.DOColor(newColor, movingTime/2));

        animationSequence.Append(
            rTransform.DOScale(scaleRate, movingTime/2));

        


    }
    private void PlaceEnemyCardToInitialPosition()
    {
        animationSequence.AppendCallback(() =>
        {
            rTransform.anchoredPosition = m_StartingAnchorPosition;
            rTransform.localScale = m_InitialScaleRate;
            directionImage.color = m_InitialColor;
            cardValue.color = m_InitialColor;
            cardImage.color = m_InitialColor;
        });

    }



    private void PlayComboAnimation()
    {

       
        MoveToCenter();
        PlayComboEffect();
    }

    private void PlayNoComboAnimation()
    {
        
        MoveToCenter();
        PlayNoComboEffect();

    }

    private void PlayNoComboEffect()
    {

        //Play Smoke Effect
        var mainParticleSys = noComboParticleSystem.main;
        if (noComboParticleSystem.isStopped)
        {
            mainParticleSys.duration = transformationTime;
        }
        animationSequence.AppendCallback(() => noComboParticleSystem.Play());

        //Hide Card in Smoke
        var newColor = cardImage.color;
        newColor.a = 0f;
        animationSequence.Append(cardImage.DOColor(newColor, transformationTime / 2));
        animationSequence.Join(directionImage.DOColor(newColor, transformationTime / 2));
        animationSequence.Join(cardValue.DOColor(newColor, transformationTime / 2));


        //Reveal item and hide smoke
        animationSequence.AppendCallback((() =>
        {
            cardImage.sprite = effectSprite;
        }));
        newColor = cardImage.color;
        newColor.a = 1f;
        animationSequence.Append(cardImage.DOColor(newColor, transformationTime / 2));
        animationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Stop();

        }));

        //Detect Direction and through the item
        var target = direction == 0 ? player : enemy;
        var position = Camera.main.WorldToScreenPoint(target.transform.position);
        animationSequence.Append(rTransform.DOMove(position, movingTime));
        animationSequence.AppendCallback(() =>
        {
            rTransform.anchoredPosition = m_StartingAnchorPosition;
            cardImage.sprite = cardSprite;
            rTransform.localScale = new Vector3(1f, 1f);
        });



    }


    private void PlayComboEffect()
    {
        //Play Smoke Particle Effect
        var mainParticleSys = noComboParticleSystem.main;
        if (noComboParticleSystem.isStopped)
        {
            mainParticleSys.duration = transformationTime;
        }
        animationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Play();
        }));


        //Hide Card in Smoke
        var newColor = cardImage.color;
        newColor.a = 0f;
        animationSequence.Append(cardImage.DOColor(newColor, transformationTime / 2));
        animationSequence.Join(directionImage.DOColor(newColor, transformationTime / 2));
        animationSequence.Join(cardValue.DOColor(newColor, transformationTime / 2));
     

        //Reveal Combo card From Smoke
        animationSequence.AppendCallback((() =>
        {
            cardImage.sprite = comboCardSprite;
        }));
        newColor = cardImage.color;
        newColor.a = 1f;
        animationSequence.Append(cardImage.DOColor(newColor, transformationTime / 2));

        animationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Stop();
            noComboParticleSystem.Clear();
        }));
        animationSequence.Join(rTransform.DOScale(new Vector3(1f, 1f), transformationTime / 2));

        //Wait for some Time
        animationSequence.AppendInterval(transformationTime / 3);

        //Hide Combo Card in smoke (for transformation to item)
        animationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Play();
        }));
        newColor = cardImage.color;
        newColor.a = 0f;
        animationSequence.Append(cardImage.DOColor(newColor, transformationTime / 2));
        animationSequence.Join(rTransform.DOScale(scaleRate, transformationTime / 2));



        //Reveal Item from smoke
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


        //Detect Direction and through the item
        var target = direction == 0 ? player : enemy;
        var finalPosition = Camera.main.WorldToScreenPoint(target.transform.position);
        animationSequence.Append(rTransform.DOMove(finalPosition, movingTime));
        animationSequence.AppendCallback(() =>
        {
            rTransform.anchoredPosition = m_StartingAnchorPosition;
            cardImage.sprite = cardSprite;
            rTransform.localScale = new Vector3(1f, 1f);
        });
    }

    private void MoveToCenter()
    {
        CardUIAnimator.enabled = false;
        
        animationSequence.Append(
            rTransform.DOAnchorPos(firstPoint.anchoredPosition, movingTime));
        animationSequence.Join(
            rTransform.DOScale(scaleRate, movingTime));
        
    }

    public void ToggleCard()
    {
        selected = !selected;
        if (selected)
        {
            rTransform.anchoredPosition += new Vector2(0, 30);
        }
        else
        {
            rTransform.anchoredPosition -= new Vector2(0, 30);
        }
        
    }




}