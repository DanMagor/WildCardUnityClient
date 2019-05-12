using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private MatchManager matchManager;

    [SerializeField]
    private GameObject menu;

    private void Start()
    {
        matchManager = GetComponent<MatchManager>();
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
        if (MatchManager.matchState != MatchManager.MatchStates.CardChoosing) return;

        if (position < 0 || position > 3)
        {
            throw new IndexOutOfRangeException("InputManager::ToggleCard(pos) - position is out of Range!");
        }


        matchManager.SendToggleCard(position);
    }

    public void MakeShoot()
    {
        if (MatchManager.matchState != MatchManager.MatchStates.CardChoosing) return;
        matchManager.MakeShot();
    }

    public void MenuButton()
    {
        menu.SetActive(!menu.activeSelf);
    }

    public void LeaveMatch()
    {
        matchManager.LeaveMatch();
    }

    
}


