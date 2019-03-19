//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class CardsUIManager : MonoBehaviour
//{


//    public SingleCardUIManager[] cardUIManagers;
//    public GameObject[] cardCup;
//    public EffectUIManager[] effectUIManagers;
//    public int offset;


//    private void Awake()
//    {
//        for (int i = 0; i < effectUIManagers.Length; i++)
//        {
//            effectUIManagers[i].effectImage.sprite = null;
//            effectUIManagers[i].durationLabel.text = "";
//            effectUIManagers[i].valueLabel.text = "";
//        }
        
//    }

//    public void ShowCards(int[] cardIDs)
//    {


//        //CardSerializable card = ClientManager.allCardsInfo[cardIDs[0]];
//        Sprite im = ClientManager.allCardsSprites[cardIDs[0]];
//        Sprite initiative_eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];
//        Sprite item_eff_im = ClientManager.allEffectsSprites[card.initiativeEffect]; //TODO: REPLACE?

//        //Hide Cover Image
//        foreach (var cup in cardCup)
//        {
//            cup.SetActive(false);
//        }


//        //Player:
//        {
           

//            //Attack Card:
//            cardUIManagers[0].cardID = card.id;
//            cardUIManagers[0].cardImage.sprite = im;
//            cardUIManagers[0].initiativeImage.sprite = initiative_eff_im;

//            if (card.initiativeValue != 0)
//            {
//                cardUIManagers[0].initiativeLabel.text = card.initiativeValue.ToString();
//            }
//            else
//            {
//                cardUIManagers[0].initiativeLabel.text = "";
//            }

//            cardUIManagers[0].bulletsLabel.text = card.bullets.ToString();
//            cardUIManagers[0].damageLabel.text = (card.damage * card.bullets).ToString();

//            //Heal Card:
//            card = ClientManager.allCardsInfo[cardIDs[1]];
//            im = ClientManager.allCardsSprites[cardIDs[1]];
//            initiative_eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];

//            cardUIManagers[1].cardID = card.id;
//            cardUIManagers[1].cardImage.sprite = im;
//            cardUIManagers[1].initiativeImage.sprite = initiative_eff_im;

//            if (card.initiativeValue != 0)
//            {
//                cardUIManagers[1].initiativeLabel.text = card.initiativeValue.ToString();
//            }
//            else
//            {
//                cardUIManagers[1].initiativeLabel.text = "";
//            }

//            cardUIManagers[1].healLabel.text = card.heal.ToString();

//            //Item Card:
//            card = ClientManager.allCardsInfo[cardIDs[2]];
//            im = ClientManager.allCardsSprites[cardIDs[2]];
//            initiative_eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];

//            cardUIManagers[2].cardID = card.id;
//            cardUIManagers[2].cardImage.sprite = im;
//            cardUIManagers[2].initiativeImage.sprite = initiative_eff_im;

//            if (card.initiativeValue != 0)
//            {
//                cardUIManagers[2].initiativeLabel.text = card.initiativeValue.ToString();
//            }
//            else
//            {
//                cardUIManagers[2].initiativeLabel.text = "";
//            }

//            cardUIManagers[2].itemDurationLabel.text = card.itemDuration.ToString();
//            cardUIManagers[2].itemEffectImage.sprite = initiative_eff_im;
//            cardUIManagers[2].itemEffectLabel.text = card.itemEffectLabel;
//        }

//        //Enemy:
//        {
//            //Attack Card:
//            card = ClientManager.allCardsInfo[cardIDs[0]];
//            im = ClientManager.allCardsSprites[cardIDs[0]];
//            initiative_eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];
//            cardUIManagers[3].cardID = card.id;
//            cardUIManagers[3].cardImage.sprite = im;
//            cardUIManagers[3].initiativeImage.sprite = initiative_eff_im;

//            if (card.initiativeValue != 0)
//            {
//                cardUIManagers[3].initiativeLabel.text = card.initiativeValue.ToString();
//            }
//            else
//            {
//                cardUIManagers[3].initiativeLabel.text = "";
//            }

//            cardUIManagers[3].bulletsLabel.text = card.bullets.ToString();
//            cardUIManagers[3].damageLabel.text = card.damage.ToString();

//            //Heal Card:
//            card = ClientManager.allCardsInfo[cardIDs[1]];
//            im = ClientManager.allCardsSprites[cardIDs[1]];
//            initiative_eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];

//            cardUIManagers[4].cardID = card.id;
//            cardUIManagers[4].cardImage.sprite = im;
//            cardUIManagers[4].initiativeImage.sprite = initiative_eff_im;

//            if (card.initiativeValue != 0)
//            {
//                cardUIManagers[4].initiativeLabel.text = card.initiativeValue.ToString();
//            }
//            else
//            {
//                cardUIManagers[4].initiativeLabel.text = "";
//            }

//            cardUIManagers[4].healLabel.text = card.heal.ToString();


//            //Item Card:
//            card = ClientManager.allCardsInfo[cardIDs[2]];
//            im = ClientManager.allCardsSprites[cardIDs[2]];
//            initiative_eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];
            

//            cardUIManagers[5].cardID = card.id;
//            cardUIManagers[5].cardImage.sprite = im;
//            cardUIManagers[5].initiativeImage.sprite = initiative_eff_im;


//            if (card.initiativeValue != 0)
//            {
//                cardUIManagers[5].initiativeLabel.text = card.initiativeValue.ToString();
//            }
//            else
//            {
//                cardUIManagers[5].initiativeLabel.text = "";
//            }

//            cardUIManagers[5].itemDurationLabel.text = card.itemDuration.ToString(); 
//            cardUIManagers[5].itemEffectImage.sprite = initiative_eff_im;
//            cardUIManagers[5].itemEffectLabel.text = card.itemEffectLabel;
//        }







//    }

//    public void HideSelectorWheels()
//    {
//        for (int i = 0; i < 3; i++)
//        {
//            cardUIManagers[i].selectionWheel.SetActive(false);
//        }
//    }

//    public void ShowEffects(ByteBuffer buffer) //byteBuffer is coming
//    {
        

//        for (int i = 0; i<effectUIManagers.Length; i++)
//        {
//            effectUIManagers[i].effectImage.sprite = null;
//            effectUIManagers[i].durationLabel.text = "";
//            effectUIManagers[i].valueLabel.text = "";
//        }
//        //For Player
//        int numberOfEffects = buffer.ReadInteger();
//        for (int i = 0; i < numberOfEffects && i<effectUIManagers.Length/2; i++)//Change condition To half of the length
//        {
//            int effID = buffer.ReadInteger();
//            effectUIManagers[i].effectImage.sprite = ClientManager.allEffectsSprites[effID];
//            effectUIManagers[i].valueLabel.text = buffer.ReadInteger().ToString();
//            effectUIManagers[i].durationLabel.text = buffer.ReadInteger().ToString();
//        }


//        //For Opponent
//        numberOfEffects = buffer.ReadInteger();
//        for (int i = 0; i < numberOfEffects && i < effectUIManagers.Length/2; i++) //Change condition To half of the length
//        {
//            int effID = buffer.ReadInteger();
//            effectUIManagers[i+effectUIManagers.Length/2].effectImage.sprite = ClientManager.allEffectsSprites[effID]; //TODO: REPLACE 1
//            effectUIManagers[i+ effectUIManagers.Length / 2].valueLabel.text = buffer.ReadInteger().ToString();
//            effectUIManagers[i+ effectUIManagers.Length / 2].durationLabel.text = buffer.ReadInteger().ToString();
//        }
//    }

//    public void HideCards(string timerText = "Get Ready for Round!")
//    {
//        foreach (var cup in cardCup)
//        {
//            cup.SetActive(true);    
//        }
        

//        GetComponent<PlayerMatchManager>().SetTimerText(timerText);
//    }
//}
