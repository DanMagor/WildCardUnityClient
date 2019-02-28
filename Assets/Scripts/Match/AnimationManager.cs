using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{


    public PlayerMatchManager playerMatchManager;

    Animator animator;
 

    private void Awake()
    {
        if (playerMatchManager == null)
        {
            playerMatchManager = GetComponent<PlayerMatchManager>();
        }
        animator = GetComponent<Animator>();
    }


    public void PlayAnimation()
    {
        if (playerMatchManager.matchBegins)
        {
            animator.Play("MatchBegins");
            playerMatchManager.matchBegins = false;
        }
        else
        {
            animator.Play("SampleAnimation");
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
