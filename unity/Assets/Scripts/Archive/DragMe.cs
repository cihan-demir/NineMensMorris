using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragMe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,  IDropHandler
{
    public bool dragOnSurfaces = true;

//    private GameObject m_DraggingIcon;
    private RectTransform m_DraggingPlane;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null) {
            Debug.Log("Canvas Null");
            return;
        }

        // We have clicked something that can be dragged.
        // What we want to do is create an icon for this.
        //        m_DraggingIcon = new GameObject("white");

        //      m_DraggingIcon.transform.SetParent(canvas.transform, false);
        //    m_DraggingIcon.transform.SetAsLastSibling();



        if (dragOnSurfaces) { 
            m_DraggingPlane = transform as RectTransform;
        }
        else { 
            m_DraggingPlane = canvas.transform as RectTransform;
        }
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        
        SetDraggedPosition(eventData);
        Debug.Log("OnBeginDrag");
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData data)
    {
        SetDraggedPosition(data);        
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }


    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop Drag");
    }
}