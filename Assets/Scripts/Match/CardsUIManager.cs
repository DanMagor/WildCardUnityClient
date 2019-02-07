using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsUIManager : MonoBehaviour
{


    public GameObject[] cardSockets;
    public Image[] cardImages;
    public Text[] bulletsLabels;
    public Text[] damageLabels;

    public int offset;


    private void Awake()
    {

        float height = cardSockets[0].GetComponent<RectTransform>().rect.height;

        cardSockets[0].GetComponent<RectTransform>().position = new Vector3(Screen.width / 4, offset + height / 2);
        cardSockets[1].GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, offset + height / 2);
        cardSockets[2].GetComponent<RectTransform>().position = new Vector3(Screen.width - Screen.width / 4, offset + height / 2);
        cardSockets[3].GetComponent<RectTransform>().position = new Vector3(Screen.width / 4, Screen.height - offset - height / 2);
        cardSockets[4].GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, Screen.height - offset - height / 2);
        cardSockets[5].GetComponent<RectTransform>().position = new Vector3(Screen.width - Screen.width / 4, Screen.height - offset - height / 2);
    }

    public void ShowCards(int[] cardIDs)
    {
        for (int i = 0; i < 3; i++)
        {

            //Get image and Card infor from ClientManager
            CardSerializable card = ClientManager.allCardsInfo[cardIDs[i]];
            Sprite im = ClientManager.allCardsSprites[cardIDs[i]];


            //Assign values to Player Cards
            cardImages[i].sprite = im;
            bulletsLabels[i].text = card.bullet.ToString();
            damageLabels[i].text = card.damage.ToString();

            //Assign values to Enemy Cards
            cardImages[i + 3].sprite = im;
            bulletsLabels[i + 3].text = card.bullet.ToString();
            damageLabels[i + 3].text = card.damage.ToString();

        }

    }
}
