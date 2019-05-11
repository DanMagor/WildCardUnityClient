using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float totalWaitingTime;
    private float timeRemains;

    private Image image;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timeRemains <=0)
        {
            this.enabled = false;
        }
        image.fillAmount = timeRemains / totalWaitingTime;
        timeRemains -= Time.deltaTime;
    }

    private void OnEnable()
    {
        timeRemains = totalWaitingTime;
    }


}
