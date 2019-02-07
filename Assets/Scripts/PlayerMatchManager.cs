using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMatchManager : MonoBehaviour
{


    public static int matchID;
    public Text timer;

    static int[] givenCards;

    public static float timeRemains;
    private static bool tickTimer = false;



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

    private static void SetReadyForRound()
    {
        ClientTCP.PACKAGE_SetReadyForRound(PlayerMatchManager.matchID);
    }



    public static void PlaceCards(int[] cards)
    {
        givenCards = new int[cards.Length]; //TODO CHECK PERFOMANCE; CHECK SIZE Requirements (const size?)
        for (int i = 0; i < cards.Length; i++)
        {
            givenCards[i] = cards[i];
        }

           
    } 

    public static void StartRound()
    {
        tickTimer = true;
        timeRemains = 3.0f;
        
        CardsUIManager.ShowCards(givenCards);

    }

    public static void ShowResult()
    {
        Debug.Log("Animation");
        SetReadyForRound();
    }
    
    
}
