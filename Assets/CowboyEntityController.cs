using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyEntityController : MonoBehaviour
{
    public string userName;
    public bool amIShot;

    public ParticleSystem Particles;
    public Animator AnimatorController;

    public void Shot()
    {
        AnimatorController.Play("Shot");
        Particles.Play();
    }

}
