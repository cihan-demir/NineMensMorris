using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Field : MonoBehaviour, IDropHandler
{

    public enum TriggerType                                                 // Types of drag and drop events
    {
        DropRequest,                                                        // Request for item drop from one cell to another
        RemoveStone,                                                       // Drop event completed
        ItemAdded,                                                          // Item manualy added into cell
        ItemWillBeDestroyed,                                                 // Called just before item will be destroyed
        EndTurn
    }

    public class DropEventDescriptor                                        // Info about item's drop event
    {
        public TriggerType triggerType;
        public Field destinationField;
        public Stone stone;
    }

    private bool hasPiece { get { return currentStone != null;} }
    public Stone currentStone { get; set; }

    //public Field field { get; set; }

    private Boolean DEBUG = true;


    public void Awake()
    {
        if (!DEBUG)
        {
            Image image = GetComponent<Image>();
            image.color = new Color(0, 0, 0, 0);
        }
    }

    public void Start()
    {
        Debug.Log("Start");
    }



    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        UpdateMyItem();
        if (eventData.pointerDrag != null && !hasPiece && !ControlUnit.game.GameEnded) {            
            Debug.Log(GetComponent<RectTransform>().anchoredPosition);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;          
            currentStone = Stone.DRAGING_STONE;

            var desc = new DropEventDescriptor {
                destinationField = this,
                stone = Stone.DRAGING_STONE,
                triggerType = TriggerType.DropRequest
            };


            SendNotification(desc);
        }
    }

    internal void AddStone(Stone stone)
    {
        currentStone = stone;
        stone.transform.SetParent(transform);        
        stone.currentField = this;
    }

    internal void StoneDropOffOnOtherField()
    {
        currentStone = null;
        //DestroyItem();
    }

    internal void RemoveItem()
    {
        
        SendNotification(new DropEventDescriptor
        {
            stone = currentStone,
            triggerType = TriggerType.RemoveStone
        });
        Debug.Log("calling destroy");
        Destroy(currentStone.gameObject);
        currentStone = null;

        SendNotification(new DropEventDescriptor
        {
            triggerType = TriggerType.EndTurn
        });

        //DestroyItem();
    }

    /// <summary>
    /// Updates my item
    /// </summary>
    public void UpdateMyItem()
    {
        currentStone = GetComponentInChildren<Stone>();
    }

    /// <summary>
    /// Send drag and drop information to application
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
    private void SendNotification(DropEventDescriptor desc)
    {
        if (desc != null)
        {
            // Send message with DragAndDrop info to parents GameObjects
            gameObject.SendMessageUpwards("OnSimpleDragAndDropEvent", desc, SendMessageOptions.DontRequireReceiver);
        }
    }
}
