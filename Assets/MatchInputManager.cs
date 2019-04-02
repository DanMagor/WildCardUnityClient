using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MatchInputManager : MonoBehaviour
{
    public ClientMatchManager matchManager;
    public UICard[] UICards; //TODO: Check do I need it?

    private void Awake()
    {
        matchManager = GetComponent<ClientMatchManager>();
    }

    public void ToggleCard(int position)
    {

        if (position < 0 || position > 3)
        {
            throw new IndexOutOfRangeException("InputManager::ToggleCard(pos) - position is out of Range!");
        }

        matchManager.SendToggleCard(position);
    }

    

    public void MakeShoot()
    {
        throw new NotImplementedException();
    }

    
}


