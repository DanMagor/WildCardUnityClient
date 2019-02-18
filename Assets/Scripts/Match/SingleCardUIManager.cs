using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleCardUIManager : MonoBehaviour {

    public int cardID;
    public Image cardImage;
    public Text bulletsLabel;
    public Text damageLabel;
    public PlayerMatchManager pm;
    public CardsUIManager cardManager;

    public GameObject selectionWheel;


    public void ToggleSelection()
    {
        cardManager.HideSelectorWheels();
        selectionWheel.SetActive(!selectionWheel.activeSelf);
    }

    public void SetSelected(string bodyPart)
    {
       
        pm.SetSelectedCardID(cardID, bodyPart);
    }
    private void Awake()
    {
       // pm = GetComponentInParent<PlayerMatchManager>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
