using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsUIManager : MonoBehaviour
{


    public SingleCardUIManager[] cardUIManagers;
    
    public int offset;


    private void Awake()
    {

        float height = cardUIManagers[0].GetComponent<RectTransform>().rect.height;

        cardUIManagers[0].GetComponent<RectTransform>().position = new Vector3(Screen.width / 4, offset + height / 2);
        cardUIManagers[1].GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, offset + height / 2);
        cardUIManagers[2].GetComponent<RectTransform>().position = new Vector3(Screen.width - Screen.width / 4, offset + height / 2);
        cardUIManagers[3].GetComponent<RectTransform>().position = new Vector3(Screen.width / 4, Screen.height - offset - height / 2);
        cardUIManagers[4].GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, Screen.height - offset - height / 2);
        cardUIManagers[5].GetComponent<RectTransform>().position = new Vector3(Screen.width - Screen.width / 4, Screen.height - offset - height / 2);
    }

    public void ShowCards(int[] cardIDs)
    {
        for (int i = 0; i < 3; i++)
        {

            //Get image and Card infor from ClientManager
            CardSerializable card = ClientManager.allCardsInfo[cardIDs[i]];
            Sprite im = ClientManager.allCardsSprites[cardIDs[i]];


            //Assign values to Player Cards
            cardUIManagers[i].cardID = card.id;
            cardUIManagers[i].cardImage.sprite = im;
            cardUIManagers[i].bulletsLabel.text = card.bullets.ToString();
            cardUIManagers[i].damageLabel.text = card.damage.ToString();

            //Assign values to Enemy Cards
            cardUIManagers[i+3].cardID = card.id;
            cardUIManagers[i+3].cardImage.sprite = im;
            cardUIManagers[i+3].bulletsLabel.text = card.bullets.ToString();
            cardUIManagers[i+3].damageLabel.text = card.damage.ToString();

        }

    }
}
