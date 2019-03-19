using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMatchManager : MonoBehaviour
{
    

    [NonSerialized]
    public bool Ready = false;

    public CardScriptableObject[] Cards;

    public AnimationManager AnimationManager;
    public CowboyEntityController PlayerEntityController;
    public CowboyEntityController EnemyEntityController;

    private void Awake()
    {
        AnimationManager = GetComponent<AnimationManager>();
    }

    public void ShowResult(ByteBuffer data)
    {
        throw new NotImplementedException();
    }

    public void SendSetReady(ByteBuffer data)
    {
        throw new NotImplementedException();
    }
    
    public void StartTimer(ByteBuffer data)
    {
        throw new NotImplementedException();
    }

    public void ShowCards(ByteBuffer data)
    {
        throw new NotImplementedException();
    }

    public void SendToggleCard(int position)
    {
        AnimationManager.ShowResult();
    }

    public void ToggleCard(ByteBuffer data)
    {
        throw new NotImplementedException();
    }
    public void MakeShoot()
    {
        throw new NotImplementedException();
    }







}
