using UnityEngine;
using UnityEngine.EventSystems;

public class Stone : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public bool whiteStone = true;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static Stone DRAGING_STONE;
    public static bool DROP_ON_FIELD = false;

    private static Vector2 STARTING_DRAG_POSITION;

    private Field _currentField;
    public Field currentField {
        get
        {
            return _currentField;
        }
        set
        {
            rectTransform.anchoredPosition = Vector3.zero;
            _currentField = value;
        }
    }

    private static Canvas canvas;
    private static string canvasName = "TempCanvas";
    private static int canvasSortOrder = 100;
    private Transform currentParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvas == null)
        {            
            GameObject canvasObj = new GameObject(canvasName);
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = canvasSortOrder;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        if (ControlUnit.game.IsInRemoveStoneState && currentField != null && ControlUnit.game.IsRemoveLegalMove(int.Parse(currentField.name)))
        {
            Debug.Log("OnPointDown RemoveItem");
            currentField.RemoveItem();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(ControlUnit.game.isWhiteTurn == whiteStone) { 
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;

            currentParent = transform.parent;
            transform.SetParent(canvas.transform);

            Debug.Log("OnBeginDrag starting position: " + rectTransform.position);
            DRAGING_STONE = this;
            STARTING_DRAG_POSITION = rectTransform.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {   
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (ControlUnit.game.isWhiteTurn == whiteStone)
        {
            Debug.Log("OnEndDrag");
            if (!DROP_ON_FIELD)
            {
                Debug.Log("OnEndDrag Reset Position");
                rectTransform.position = STARTING_DRAG_POSITION;
                DRAGING_STONE = null;
                transform.SetParent(currentParent);
            }            
        }
        DROP_ON_FIELD = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ControlUnit.game.isWhiteTurn == whiteStone)
        {
            Debug.Log("OnDrag");

            rectTransform.position = Input.mousePosition;
        }
    }
}
