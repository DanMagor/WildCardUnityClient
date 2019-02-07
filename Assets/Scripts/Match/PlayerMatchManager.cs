using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMatchManager : MonoBehaviour
{

    public string playerUsername;
    public string enemyUsername;
    public Text playerUsernameLabel;
    public Text enemyUsernameLabel;

    public CardsUIManager cardsUIManager;
    public AnimationManager animationManager;


    public int matchID;
    public Text timer;

    int[] givenCards;

    int selectedCardID;

    public float timeRemains;
    private bool tickTimer = false;


    //TODO: Replace in Sub Object
    public Animator sampleAnimation;



    //INITIALISATION
    public void InitializeLabels(string pUsername, string eUsername)
    {
        playerUsername = pUsername;
        enemyUsername = eUsername;
        playerUsernameLabel.text = playerUsername;
        enemyUsernameLabel.text = enemyUsername;
    }
    public void Awake()
    {
        if (cardsUIManager == null)
        {
            cardsUIManager = GetComponent<CardsUIManager>();
        }
        if (animationManager == null)
        {
            animationManager = GetComponent<AnimationManager>();
        }

    }

    //TODO: DELETE GET MOUSE TEMPORARY:

    private void Update()
    {

        if (tickTimer)
        {

            timeRemains -= Time.deltaTime;
            timer.text = Mathf.CeilToInt(timeRemains).ToString();
            if (timeRemains <= 0)
            {
                tickTimer = false;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetReadyForRound();
        }
    }

    public void SetReadyForRound()
    {
        ClientTCP.PACKAGE_SetReadyForRound(matchID);
    }



    public void PlaceCards(int[] cards)
    {
        givenCards = new int[cards.Length]; //TODO CHECK PERFOMANCE; CHECK SIZE Requirements (const size?)
        for (int i = 0; i < cards.Length; i++)
        {
            givenCards[i] = cards[i];
        }


    }

    public void StartRound()
    {
        tickTimer = true;
        timeRemains = 3.0f;

        cardsUIManager.ShowCards(givenCards);

    }

    public void ShowResult()
    {
        string animationName = "SampleAnimation";
        sampleAnimation.Play(animationName);
        
    }

    public void SetSelectedCardID(int cardID)
    {
        selectedCardID = cardID;
        ClientTCP.PACKAGE_SendSelectedCard(matchID, selectedCardID);
    }




}
