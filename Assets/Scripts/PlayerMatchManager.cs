using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMatchManager : MonoBehaviour
{


    public static int matchID;


    //TODO: DELETE, TEMPORARY:
    public static void PrintCard(string cardName, Card.CardTypes cardType, int damage, string username)
    {
        Debug.Log("Card: '" + cardName + "' of type '" + cardType + "' with " + damage + " damage received to " + username);

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
