using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler
{

    private Boolean hasPiece = false;

    public void Start()
    {
        Debug.Log("Start");
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if(eventData.pointerDrag != null && !hasPiece) {
            Debug.Log("eventData anchor: " + eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition);
            Debug.Log("eventData anchor: " + GetComponent<RectTransform>().anchoredPosition);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            hasPiece = true;
        }
    }
}
