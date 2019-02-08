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

    public void SetSelected()
    {
        pm.SetSelectedCardID(cardID);
    }
    private void Awake()
    {
        pm = GetComponentInParent<PlayerMatchManager>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
