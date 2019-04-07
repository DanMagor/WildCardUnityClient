using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MatchInputManager : MonoBehaviour
{
    public ClientMatchManager matchManager;

    private void Awake()
    {
       
    }

    private void Start()
    {
        matchManager = GetComponent<ClientMatchManager>();
    }
    private void Update()
    {

        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Restart");
            matchManager.RestartMatch();
        }
    }

    public void ToggleCard(int position)
    {
        if (ClientMatchManager.matchState != ClientMatchManager.MatchStates.CardChoosing) return;

        if (position < 0 || position > 3)
        {
            throw new IndexOutOfRangeException("InputManager::ToggleCard(pos) - position is out of Range!");
        }


        matchManager.ToggleCard(position);
    }

  

    public void MakeShoot()
    {
        if (ClientMatchManager.matchState != ClientMatchManager.MatchStates.CardChoosing) return;
        matchManager.MakeShoot();

    }

    
}


