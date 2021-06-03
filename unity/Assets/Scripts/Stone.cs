using UnityEngine;
using UnityEngine.EventSystems;

public class Stone : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
  public bool WhiteStone = true;
  private RectTransform rectTransform;
  private CanvasGroup canvasGroup;

  public static Stone DRAGING_STONE;
  public static bool DROP_ON_FIELD = false;

  private static Vector2 STARTING_DRAG_POSITION;

  private Field _currentField;
  public Field CurrentField
  {
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
  private Transform _currentParent;
  private Transform _originalParentTransform;
  private Vector3 _originalPosition;

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
    _originalParentTransform = transform.parent.transform;
    _originalPosition = transform.localPosition;
  }

  public void HideStone()
  {
    gameObject.SetActive(false);
    transform.SetParent(_originalParentTransform);
    transform.localPosition = _originalPosition;
  }

  public void ResetStone()
  {
    CurrentField = null;
    transform.SetParent(_originalParentTransform);
    transform.localPosition = _originalPosition;
    gameObject.SetActive(true);
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    //Debug.Log("OnPointerDown");
    if (ControlUnit.game.IsInRemoveStoneState && CurrentField != null && ControlUnit.game.IsRemoveLegalMove(int.Parse(CurrentField.name)))
    {
      //Debug.Log("OnPointDown RemoveItem");
      CurrentField.RemoveItem();
    }
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    if (ControlUnit.game.isWhiteTurn == WhiteStone)
    {
      canvasGroup.alpha = 0.6f;
      canvasGroup.blocksRaycasts = false;

      _currentParent = transform.parent;
      transform.SetParent(canvas.transform);

      //Debug.Log("OnBeginDrag starting position: " + rectTransform.position);
      DRAGING_STONE = this;
      STARTING_DRAG_POSITION = rectTransform.position;
    }
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    canvasGroup.alpha = 1f;
    canvasGroup.blocksRaycasts = true;
    if (ControlUnit.game.isWhiteTurn == WhiteStone)
    {
      //Debug.Log("OnEndDrag");
      if (!DROP_ON_FIELD)
      {
        //Debug.Log("OnEndDrag Reset Position");
        rectTransform.position = STARTING_DRAG_POSITION;
        DRAGING_STONE = null;
        transform.SetParent(_currentParent);
      }
    }
    DROP_ON_FIELD = false;
  }

  public void OnDrag(PointerEventData eventData)
  {
    if (ControlUnit.game.isWhiteTurn == WhiteStone)
    {
      //Debug.Log("OnDrag");

      rectTransform.position = Input.mousePosition;
    }
  }
}