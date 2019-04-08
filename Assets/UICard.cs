using DG.Tweening;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICard : MonoBehaviour, IPointerClickHandler
{
    public static float movingTime = 3f;//0.3f
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

    private void Start()
    {
    }

    private void Update()
    {

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
        //TODO: IMPORTANT! maybe delete later
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
            rTransform.DOAnchorPos(firstPoint.anchoredPosition, movingTime));
        animationSequence.Join(
            rTransform.DOScale(new Vector3(1f, 1f, 1f), movingTime));
        var newColor = m_InitialColor;
        newColor.a = 1;
        var textColor = cardValue.color;
        textColor.a = 1;
        animationSequence.Join(cardValue.DOColor(textColor, movingTime / 2));
        animationSequence.Join(cardImage.DOColor(newColor, movingTime / 2));
           //  animationSequence.Join(directionImage.DOColor(newColor, movingTime/2));

        animationSequence.Append(
            rTransform.DOScale(scaleRate, movingTime));

        newColor = cardImage.color;
        newColor.a = 0f;
        animationSequence.Join(directionImage.DOColor(newColor, movingTime));
        animationSequence.Join(cardValue.DOColor(newColor, movingTime));


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
        var mainParticleSys = noComboParticleSystem.main;
        if (noComboParticleSystem.isStopped)
        {
            mainParticleSys.duration = transformationTime;
        }



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

        var mainParticleSys = noComboParticleSystem.main;
        if (noComboParticleSystem.isStopped)
        {
            mainParticleSys.duration = transformationTime;
        }




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

        var newColor = cardImage.color;
        newColor.a = 0f;
        animationSequence.Join(directionImage.DOColor(newColor, movingTime));
        animationSequence.Join(cardValue.DOColor(newColor, movingTime));
    }





}