using UnityEngine;
using UnityEngine.EventSystems;

public class Stone : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
  public static Stone DraggingStone;
  public static bool DropOnField = false;

  public bool WhiteStone = true;
  public RectTransform RectTransform;
  public CanvasGroup CanvasGroup;

  private Field _currentField;
  public Field CurrentField
  {
    get
    {
      return _currentField;
    }
    set
    {
      RectTransform.anchoredPosition = Vector3.zero;
      _currentField = value;
    }
  }


  public bool IsOnBoard
  {
    get { return transform.parent != _originalParentTransform; }
  }

  private static Vector2 _startingDragPosition;
  private Canvas _canvas;
  private string _canvasName = "TempCanvas";
  private int _canvasSortOrder = 100;
  private Transform _currentParent;
  private Transform _originalParentTransform;
  private Vector3 _originalPosition;

  private void Awake()
  {
    if (_canvas == null)
    {
      GameObject canvasObj = new GameObject(_canvasName);
      _canvas = canvasObj.AddComponent<Canvas>();
      _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      _canvas.sortingOrder = _canvasSortOrder;
    }
    _originalParentTransform = transform.parent.transform;
    _originalPosition = transform.localPosition;
  }

  public void HideStone()
  {
    transform.SetParent(_originalParentTransform);
    transform.localPosition = _originalPosition;
    gameObject.SetActive(false);
  }

  public void UpdateStone(int fromFieldIndex, int toFieldIndex )
  {
    //Debug.Log("OnPointerDown");
    if (ControlUnit.Game.IsInRemoveStoneState && CurrentField != null)
    {
      var currentFieldIndex = int.Parse(CurrentField.name);
      if (currentFieldIndex == toFieldIndex)
      {
        //Debug.Log("Hide this stone: " + gameObject.name);
        HideStone();
        CurrentField.currentStone = null;
      }
    }

    ResetStone(false);
  }

  public void ResetStone(bool show = true)
  {
    //Debug.Log(gameObject.name + " Reset stone: " + show);
    CurrentField = null;
    transform.SetParent(_originalParentTransform);
    transform.localPosition = _originalPosition;
    if (show)
      gameObject.SetActive(true);
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    //Debug.Log("OnPointerDown");
    if (ControlUnit.Game.IsInRemoveStoneState && CurrentField != null && ControlUnit.Game.IsRemoveLegalMove(int.Parse(CurrentField.name)))
    {
      //Debug.Log("OnPointDown RemoveItem");
      CurrentField.RemoveItem();
    }
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    if (ControlUnit.Game.isWhiteTurn == WhiteStone)
    {
      CanvasGroup.alpha = 0.6f;
      CanvasGroup.blocksRaycasts = false;

      _currentParent = transform.parent;
      transform.SetParent(_canvas.transform);

      //Debug.Log("OnBeginDrag starting position: " + rectTransform.position);
      DraggingStone = this;
      _startingDragPosition = RectTransform.position;
    }
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    CanvasGroup.alpha = 1f;
    CanvasGroup.blocksRaycasts = true;
    if (ControlUnit.Game.isWhiteTurn == WhiteStone)
    {
      //Debug.Log("OnEndDrag");
      if (!DropOnField)
      {
        //Debug.Log("OnEndDrag Reset Position");
        RectTransform.position = _startingDragPosition;
        DraggingStone = null;
        transform.SetParent(_currentParent);
      }
    }
    DropOnField = false;
  }

  public void OnDrag(PointerEventData eventData)
  {
    if (ControlUnit.Game.isWhiteTurn == WhiteStone)
    {
      //Debug.Log("OnDrag");

      RectTransform.position = Input.mousePosition;
    }
  }
}