using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private Canvas canvas;

    //private Sprite piece;
    private RectTransform rectTransform;
    private RectTransform m_DraggingPlane;
    private CanvasGroup canvasGroup;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        //piece = GetComponent<Sprite>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        Debug.Log("OnBeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        Debug.Log("OnEndDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_DraggingPlane = transform as RectTransform;
        Debug.Log("OnEndDrag1 " + canvas.scaleFactor + " " + eventData.delta );
        
        Debug.Log("OnEndDrag2 " + canvas.scaleFactor + " " + eventData.delta / canvas.scaleFactor);
        Debug.Log("OnEndDrag3 " + rectTransform.anchoredPosition + " " + canvas.scaleFactor + " " + eventData.delta * canvas.scaleFactor);

        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos;
            rectTransform.rotation = m_DraggingPlane.rotation;
        }

        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor / 3;
        Debug.Log(rectTransform.anchoredPosition);
        //SetDraggedPosition(eventData);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        //var rt = m_DraggingIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos;
            rectTransform.rotation = m_DraggingPlane.rotation;
        }
    }
}
