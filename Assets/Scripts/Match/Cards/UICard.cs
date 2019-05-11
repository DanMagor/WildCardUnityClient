using DG.Tweening;
using System;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICard : MonoBehaviour, IPointerClickHandler
{
    #region Animation Parameters
    //Time
    [SerializeField]
    private float movingTime = 0.5f; //0.3f
    [SerializeField]
    private float transformationTime = 0.5f;
    [SerializeField]
    private float hidingShowingTime = 0.1f;
    [SerializeField]
    private float directionChangeTime = 0.7f;

    //Scale
    [SerializeField]
    private Vector3 scaleRate = new Vector3(0.3f, 0.3f);
    [SerializeField]
    private Vector2 imageRectSize = new Vector2(315f, 315f);

    //Targets for flying item
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;

    //Target Point in the middle of the screen
    public RectTransform CenterPoint;

    //Particle System for Card Transformation
    [SerializeField]
    private ParticleSystem noComboParticleSystem;
    #endregion

    #region Initial Position and Rotation
    [SerializeField]
    private int Position; //From 0 to 3
    private RectTransform rTransform;
    private Vector3 m_StartingAnchorPosition;
    private Color m_InitialColor;
    private Vector3 m_InitialScaleRate;
    private Vector2 m_InitialRtSize;
    private Vector3 m_HiddenPosition;
    private Vector3 m_ShownPosition;
    #endregion

    #region Images and Sprites
    //Public Available Field For Image Changing from Animation Manager
    public Sprite CardSprite;
    public Sprite ComboCardSprite;
    public Sprite ItemSprite;

    public Image CardImage;
    public Image DirectionImage;
    public Text CardValue;

    #endregion

    //Animation Sequence for current Animation Manager
    public Sequence AnimationSequence;
   
    //Public Fields For Animation Manager
    [NonSerialized]
    public bool Selected = false;
    [NonSerialized]
    public int direction;

    //Click Detecting
    public InputManager inputManager;

    //Detect Card Click
    public void OnPointerClick(PointerEventData eventData)
    {
        inputManager.ToggleCard(Position);
    }

    public void Awake()
    {
        inputManager = GetComponentInParent<InputManager>();
       
        rTransform = GetComponent<RectTransform>();
        CardImage = GetComponent<Image>();
        CardValue = GetComponentInChildren<Text>();

      
        m_StartingAnchorPosition = rTransform.anchoredPosition;
        m_InitialColor = CardImage.color;
        m_InitialScaleRate = rTransform.localScale;
        m_InitialRtSize = rTransform.sizeDelta;

        m_HiddenPosition = m_StartingAnchorPosition;
        m_ShownPosition = m_StartingAnchorPosition + new Vector3(0, rTransform.rect.height, 0);
    }

    #region Public For Animation Manager
    public void PlayAnimation(string animationName)
    {
        DOTween.defaultAutoPlay = AutoPlay.AutoPlaySequences;
        AnimationSequence = DOTween.Sequence();
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
        var newColor = DirectionImage.color;
        newColor.a = 1f;
        //TODO: IMPORTANT! maybe delete later //Setup default Colors for Cards
        AnimationSequence.Append(CardValue.DOColor(newColor, 0));
        AnimationSequence.Append(DirectionImage.DOColor(newColor, 0));
    }
    public void ToggleCardSelection()
    {
        Selected = !Selected;
        if (Selected)
        {
            rTransform.anchoredPosition += new Vector2(0, 30);
        }
        else
        {
            rTransform.anchoredPosition -= new Vector2(0, 30);
        }

    }
    public void PlayShowCard()
    {
        AnimationSequence = DOTween.Sequence();

        AnimationSequence.Append(rTransform.DOAnchorPos(m_ShownPosition, hidingShowingTime));
    }
    #endregion

    private void PlayNotSelectedCard()
    {

        AnimationSequence.Append(DirectionImage.gameObject.GetComponent<RectTransform>().DORotate(new Vector3(0, 180, 0), directionChangeTime));
        AnimationSequence.Append(rTransform.DOAnchorPos(m_HiddenPosition, hidingShowingTime));
        AnimationSequence.Append(DirectionImage.gameObject.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 0), 0f));
    }

    #region General Animations
    private void MoveToCenter()
    {
        AnimationSequence.Append(
            rTransform.DOAnchorPos(CenterPoint.anchoredPosition, movingTime));
        AnimationSequence.Join(
            rTransform.DOScale(scaleRate, movingTime));
    }
    private void ThrowItemtInPlayer()
    {

        //Reveal Item from smoke
        var newColor = CardImage.color;
        newColor.a = 1f;
        AnimationSequence.AppendCallback((() =>
        {
            CardImage.sprite = ItemSprite;

            //set size for Item Image
            rTransform.sizeDelta = imageRectSize;

        }));
        AnimationSequence.Append(CardImage.DOColor(newColor, transformationTime / 2));
        AnimationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Stop();
        }));


        //Detect direction and through the item
        var target = direction == 0 ? player : enemy;
        var finalPosition = Camera.main.WorldToScreenPoint(target.transform.position);
        AnimationSequence.Append(rTransform.DOMove(finalPosition, movingTime));
        AnimationSequence.AppendCallback(() =>
        {
            rTransform.anchoredPosition = m_StartingAnchorPosition;
            CardImage.sprite = CardSprite;
            rTransform.localScale = new Vector3(1f, 1f);
            rTransform.sizeDelta = m_InitialRtSize;
        });
    }
    #endregion

    #region NoCombo Animation
    private void PlayNoComboAnimation()
    {
        MoveToCenter();
        PlayNoComboTransformation();
    }
    private void PlayNoComboTransformation()
    {

        //Play Smoke Effect
        var mainParticleSys = noComboParticleSystem.main;
        if (noComboParticleSystem.isStopped)
        {
            mainParticleSys.duration = transformationTime;
        }
        AnimationSequence.AppendCallback(() => noComboParticleSystem.Play());

        //Hide Card in Smoke
        var newColor = CardImage.color;
        newColor.a = 0f;
        AnimationSequence.Append(CardImage.DOColor(newColor, transformationTime / 2));
        AnimationSequence.Join(DirectionImage.DOColor(newColor, transformationTime / 2));
        AnimationSequence.Join(CardValue.DOColor(newColor, transformationTime / 2));

        ThrowItemtInPlayer();
    }
    #endregion

    #region Combo Animation
    private void PlayComboAnimation()
    {
        MoveToCenter();
        PlayComboTransformation();
    }
    private void PlayComboTransformation()
    {
        //Play Smoke Particle Effect
        var mainParticleSys = noComboParticleSystem.main;
        if (noComboParticleSystem.isStopped)
        {
            mainParticleSys.duration = transformationTime;
        }
        AnimationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Play();
        }));


        //Hide Card in Smoke
        var newColor = CardImage.color;
        newColor.a = 0f;
        AnimationSequence.Append(CardImage.DOColor(newColor, transformationTime / 2));
        AnimationSequence.Join(DirectionImage.DOColor(newColor, transformationTime / 2));
        AnimationSequence.Join(CardValue.DOColor(newColor, transformationTime / 2));


        //Reveal Combo card From Smoke
        AnimationSequence.AppendCallback((() =>
        {
            CardImage.sprite = ComboCardSprite;
        }));
        newColor = CardImage.color;
        newColor.a = 1f;
        AnimationSequence.Append(CardImage.DOColor(newColor, transformationTime / 2));

        AnimationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Stop();
            noComboParticleSystem.Clear();
        }));
        AnimationSequence.Join(rTransform.DOScale(new Vector3(1f, 1f), transformationTime / 2));

        //Wait for some Time
        AnimationSequence.AppendInterval(transformationTime / 3);

        //Hide Combo Card in smoke (for transformation to item)
        AnimationSequence.AppendCallback((() =>
        {
            noComboParticleSystem.Play();
        }));
        newColor = CardImage.color;
        newColor.a = 0f;
        AnimationSequence.Append(CardImage.DOColor(newColor, transformationTime / 2));
        AnimationSequence.Join(rTransform.DOScale(scaleRate, transformationTime / 2));


        ThrowItemtInPlayer();


    }
    #endregion

    #region Enemy Cards Animation
    private void PlayEnemyCardAnimation()
    {

        MoveToCenterEnemyCard();
        PlayNoComboTransformation();
        PlaceEnemyCardToInitialPosition();

    }
    private void MoveToCenterEnemyCard()
    {




        CardImage.sprite = CardSprite;


        AnimationSequence.Append(
            rTransform.DOAnchorPos(CenterPoint.anchoredPosition, movingTime / 2));
        AnimationSequence.Join(
            rTransform.DOScale(new Vector3(1f, 1f, 1f), movingTime / 2));
        var newColor = m_InitialColor;
        newColor.a = 1;
        var textColor = CardValue.color;
        textColor.a = 1;
        AnimationSequence.Join(CardValue.DOColor(textColor, movingTime / 2));
        AnimationSequence.Join(CardImage.DOColor(newColor, movingTime / 2));
        //  animationSequence.Join(directionImage.DOColor(newColor, movingTime/2));

        AnimationSequence.Append(
            rTransform.DOScale(scaleRate, movingTime / 2));




    }
    private void PlaceEnemyCardToInitialPosition()
    {
        AnimationSequence.AppendCallback(() =>
        {
            rTransform.anchoredPosition = m_StartingAnchorPosition;
            rTransform.localScale = m_InitialScaleRate;
            DirectionImage.color = m_InitialColor;
            CardValue.color = m_InitialColor;
            CardImage.color = m_InitialColor;
        });

    }
    #endregion



   

   



}