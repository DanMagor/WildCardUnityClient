using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionUIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IDropHandler {

     RectTransform rectTransform;
     SingleCardUIManager cardUIManager;

    public string bodyPart;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cardUIManager = GetComponentInParent<SingleCardUIManager>();
    }

    public void SetBigSize()
    {
        rectTransform.localScale = new Vector3(3f, 3f);
    }

    public void SetStandartSize()
    {
        rectTransform.localScale = new Vector3(1.0f, 1.0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetBigSize();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetStandartSize();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       // cardUIManager.SetSelected(bodyPart);
    }

    public void OnDrop(PointerEventData eventData)
    {
        cardUIManager.SetSelected(bodyPart);
    }
}
