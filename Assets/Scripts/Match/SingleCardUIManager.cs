using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleCardUIManager : MonoBehaviour {

    public int cardID;
    public Image cardImage;

    public Image initiativeImage;
    public Text initiativeLabel;
    
    //For Attack Cards
    public Text bulletsLabel;
    public Text damageLabel;



    //For Heal Cards
    public Text healLabel;

    //For Item Cards
    public Text itemDurationLabel;
    public Image itemEffectImage;
    public Text itemEffectLabel;
    



    public PlayerMatchManager pm;
    public CardsUIManager cardManager;

    public GameObject selectionWheel;


    public void ToggleSelection()
    {
        if (selectionWheel.activeSelf)
        {
            selectionWheel.SetActive(false);
        }
        else
        {
            cardManager.HideSelectorWheels();
            selectionWheel.SetActive(!selectionWheel.activeSelf);
        }
    }

    public void SetSelected(string bodyPart)
    {

        cardManager.HideSelectorWheels();
        pm.SetSelectedCardID(cardID, bodyPart);
    }
    private void Awake()
    {
      //  pm = GetComponentInParent<PlayerMatchManager>();
        //cardManager = GetComponentInParent<CardsUIManager>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
