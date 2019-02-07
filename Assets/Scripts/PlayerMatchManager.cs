using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMatchManager : MonoBehaviour
{


    public int matchID;
    public Text timer;

    int[] givenCards;

    int selectedCardID;

    public float timeRemains;
    private bool tickTimer = false;

    public Animator sampleAnimation;



    //TODO: DELETE, TEMPORARY:
    private void Update()
    {

        if (tickTimer)
        {

            timeRemains -= Time.deltaTime;
            timer.text = Mathf.CeilToInt(timeRemains).ToString();
            if (timeRemains <= 0)
            {
                tickTimer = false;
                //ClientTCP.PACKAGE_SetReadyForRound(PlayerMatchManager.matchID);

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

        CardsUIManager.ShowCards(givenCards);

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
