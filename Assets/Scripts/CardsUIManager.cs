using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsUIManager : MonoBehaviour
{

    
    private static GameObject[] cardSockets;

    public GameObject[] realCardsSockets;
    public Image[] cardImages;


    public int offset = 10;


    private void Awake()
    {

        //TODO: THINK ABOUT STATIC. Is it efficient enough?
        cardSockets = new GameObject[6];
        for (int i =0; i<6; i++)
        {
            cardSockets[i] = realCardsSockets[i];
        }
        

        //cardSockets = GameObject.FindGameObjectsWithTag("CardSocket");
        float height = cardSockets[0].GetComponent<RectTransform>().rect.height;

        cardSockets[0].GetComponent<RectTransform>().position = new Vector3(Screen.width / 4, offset + height/2);
        cardSockets[1].GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, offset+ height/2);
        cardSockets[2].GetComponent<RectTransform>().position = new Vector3(Screen.width - Screen.width / 4, offset + height / 2);

        
        cardSockets[3].GetComponent<RectTransform>().position = new Vector3(Screen.width / 4, Screen.height - offset - height/2);
        cardSockets[4].GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, Screen.height - offset - height/2);
        cardSockets[5].GetComponent<RectTransform>().position = new Vector3(Screen.width - Screen.width / 4, Screen.height - offset - height/2);
        

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
            cardSockets[i].GetComponentsInChildren<Text>()[0].text = card.bullet.ToString();
            cardSockets[i].GetComponentsInChildren<Text>()[1].text = card.damage.ToString();


            cardSockets[i+3].GetComponentInChildren<Image>().sprite = im;
            cardSockets[i+3].GetComponentsInChildren<Text>()[0].text = card.bullet.ToString();
            cardSockets[i+3].GetComponentsInChildren<Text>()[1].text = card.damage.ToString();


        }

       
    }
}
