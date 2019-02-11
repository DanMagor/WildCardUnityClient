using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{


    public PlayerMatchManager playerMatchManager;
 

    private void Awake()
    {
        if (playerMatchManager == null)
        {
            playerMatchManager = GetComponent<PlayerMatchManager>();
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
