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
    }

    public void ShowCards(int[] cardIDs)
    {

        CardSerializable card = ClientManager.allCardsInfo[cardIDs[0]];
        Sprite im = ClientManager.allCardsSprites[cardIDs[0]];
        Sprite eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];


        //Player:
        {
            //Attack Card:
            cardUIManagers[0].cardID = card.id;
            cardUIManagers[0].cardImage.sprite = im;
            cardUIManagers[0].initiativeImage.sprite = eff_im;
            cardUIManagers[0].initiativeLabel.text = card.initiativeValue.ToString();

            cardUIManagers[0].bulletsLabel.text = card.bullets.ToString();
            cardUIManagers[0].damageLabel.text = card.damage.ToString();

            //Heal Card:
            card = ClientManager.allCardsInfo[cardIDs[1]];
            im = ClientManager.allCardsSprites[cardIDs[1]];
            eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];

            cardUIManagers[1].cardID = card.id;
            cardUIManagers[1].cardImage.sprite = im;
            cardUIManagers[1].initiativeImage.sprite = eff_im;
            cardUIManagers[1].initiativeLabel.text = card.initiativeValue.ToString();

            cardUIManagers[1].healLabel.text = card.heal.ToString();

            //Item Card:
            card = ClientManager.allCardsInfo[cardIDs[2]];
            im = ClientManager.allCardsSprites[cardIDs[2]];
            eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];

            cardUIManagers[2].cardID = card.id;
            cardUIManagers[2].cardImage.sprite = im;
            cardUIManagers[2].initiativeImage.sprite = eff_im;
            cardUIManagers[2].initiativeLabel.text = card.initiativeValue.ToString();

            cardUIManagers[2].itemDurationLabel.text = card.heal.ToString();
            cardUIManagers[2].itemEffectImage.sprite = eff_im;
            cardUIManagers[2].itemEffectLabel.text = card.additionalEffectValue.ToString();
        }

        //Enemy:
        {
            //Attack Card:
            card = ClientManager.allCardsInfo[cardIDs[0]];
            im = ClientManager.allCardsSprites[cardIDs[0]];
            eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];
            cardUIManagers[3].cardID = card.id;
            cardUIManagers[3].cardImage.sprite = im;
            cardUIManagers[3].initiativeImage.sprite = eff_im;
            cardUIManagers[3].initiativeLabel.text = card.initiativeValue.ToString();

            cardUIManagers[3].bulletsLabel.text = card.bullets.ToString();
            cardUIManagers[3].damageLabel.text = card.damage.ToString();

            //Heal Card:
            card = ClientManager.allCardsInfo[cardIDs[1]];
            im = ClientManager.allCardsSprites[cardIDs[1]];
            eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];

            cardUIManagers[4].cardID = card.id;
            cardUIManagers[4].cardImage.sprite = im;
            cardUIManagers[4].initiativeImage.sprite = eff_im;
            cardUIManagers[4].initiativeLabel.text = card.initiativeValue.ToString();

            cardUIManagers[4].healLabel.text = card.heal.ToString();


            //Item Card:
            card = ClientManager.allCardsInfo[cardIDs[2]];
            im = ClientManager.allCardsSprites[cardIDs[2]];
            eff_im = ClientManager.allEffectsSprites[card.initiativeEffect];

            cardUIManagers[5].cardID = card.id;
            cardUIManagers[5].cardImage.sprite = im;
            cardUIManagers[5].initiativeImage.sprite = eff_im;
            cardUIManagers[5].initiativeLabel.text = card.initiativeValue.ToString();

            cardUIManagers[5].itemDurationLabel.text = card.additionalEffectDuration.ToString(); 
            cardUIManagers[5].itemEffectImage.sprite = eff_im;
            cardUIManagers[5].itemEffectLabel.text = card.additionalEffectValue.ToString();
        }







    }

    public void HideSelectorWheels()
    {
        for (int i = 0; i < 3; i++)
        {
            cardUIManagers[i].selectionWheel.SetActive(false);
        }
    }
}
