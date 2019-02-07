using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour {


    //TEMP CLASS FOR ANIMATION TESTING< CHANGE LATER TO SEPRATE ANIMATION CONTROLLER OBJECT;

    public PlayerMatchManager playerMatchManager;


    public void FinishShowResult()
    {
        playerMatchManager.SetReadyForRound();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
