using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsUIManager : MonoBehaviour
{

        private static GameObject[] cardSockets;

    public int offset = 30;


    private void Awake()
    {
        cardSockets = GameObject.FindGameObjectsWithTag("CardSocket");
        cardSockets[0].GetComponent<RectTransform>().position = new Vector3(Screen.width / 4, offset);
        cardSockets[1].GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, offset);
        cardSockets[2].GetComponent<RectTransform>().position = new Vector3(Screen.width - Screen.width / 4, offset);
        cardSockets[3].GetComponent<RectTransform>().position = new Vector3(Screen.width / 4, Screen.height - offset);
        cardSockets[4].GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, Screen.height - offset);
        cardSockets[5].GetComponent<RectTransform>().position = new Vector3(Screen.width - Screen.width / 4, Screen.height - offset);


    }

    public static void ShowCards(int[] cardIDs)
    {
        for (int i = 0; i < 3; i++)
        {
            string path = Application.dataPath + @"\Resources\Cards\Card" + cardIDs[i].ToString(); //TODO Change to more effective Way or rearrange files


            var json = System.IO.File.ReadAllText(path);

            var card = JsonUtility.FromJson<CardSerializable>(json);

            Sprite im = Resources.Load<Sprite>(@"Cards\" + card.image);
            cardSockets[i].GetComponentInChildren<Image>().sprite = im;
            cardSockets[i].GetComponentInChildren<Text>().text = card.damage.ToString();

            cardSockets[i + 3].GetComponentInChildren<Image>().sprite = im;
            cardSockets[i+3].GetComponentInChildren<Text>().text = card.damage.ToString();


        }
    }
}
