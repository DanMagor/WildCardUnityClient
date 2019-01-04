using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private bool searchingOpponent = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void StartGame()
    {
        searchingOpponent = true;
    }

	// #TODO: Implement Logic
	public void CreateLobby()
	{
		Debug.Log("CreateLobby");
	}

	// #TODO: Implement Logic
	public void ConnectToLobby()
	{
		Debug.Log("ConnectToLobby");
	}
}
