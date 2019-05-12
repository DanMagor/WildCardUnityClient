using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnMatchMenu : MonoBehaviour, IPointerClickHandler
{
    
    // Start is called before the first frame update

    [SerializeField]
    private InputManager inputManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        inputManager.MenuButton();
    }
}
