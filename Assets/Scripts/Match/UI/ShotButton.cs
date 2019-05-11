using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShotButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private InputManager inputManager;
    // Start is called before the first frame update
   
    public void OnPointerClick(PointerEventData eventData)
    {
        inputManager.MakeShoot();
    }
}
