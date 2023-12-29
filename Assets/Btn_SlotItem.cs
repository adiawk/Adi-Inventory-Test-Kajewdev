using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Btn_SlotItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IDropHandler
{
    [HideInInspector] public bool isQuickSlot;
    [HideInInspector] public int slotIndex;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    bool isDragging = false;

    [SerializeField] Image imgIcon;
    [SerializeField] TextMeshProUGUI textAmount;
    [SerializeField] GameObject locked;

    [SerializeField] Sprite defaultIcon;
    [SerializeField] string defaultTextAmount;

    public ItemOwned currentItem;

    UIDraggedItem dragIcon;

    public Action<ItemOwned> OnSelected;


    private void Awake()
    {
        //TryGetComponent(out btn);
        TryGetComponent(out rectTransform);
        TryGetComponent(out canvasGroup);

        //defaultIcon = imgIcon.sprite;
        //defaultTextAmount = textAmount.text;
    }

    private void Start()
    {
        //if(btn != null)
        //{
        //    btn.onClick.AddListener(SelectItem);
        //} 
    }

    public void SetLocked()
    {
        imgIcon.gameObject.SetActive(false);
        textAmount.gameObject.SetActive(false);
        locked.gameObject.SetActive(true);
    }

    public void SetUnlocked()
    {
        imgIcon.gameObject.SetActive(true);
        textAmount.gameObject.SetActive(true);
        locked.gameObject.SetActive(false);
    }

    public void SetItem(ItemOwned itemOwned)
    {
        try
        {
            imgIcon.sprite = itemOwned.itemData.icon;
            textAmount.text = itemOwned.amount.ToString();
        }
        catch { Debug.Log($"Item: {itemOwned}"); }
        

        currentItem = itemOwned;
    }

    public void SetEmpty()
    {
        imgIcon.sprite = defaultIcon;
        textAmount.text = defaultTextAmount;

        currentItem = new ItemOwned
        {
            itemData = null,
            amount = 0,
            isInQuickSlot = isQuickSlot,
            slotIndex = slotIndex
        };
    }

    public void SelectItem()
    {
        if (!isDragging)
        {
            OnSelected?.Invoke(currentItem);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentItem.itemData == null)
        {
            OnSelected?.Invoke(null);
            return;
        }

        canvasGroup.blocksRaycasts = false;
        isDragging = false;

        SelectItem();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentItem.itemData == null)
            return;

        // Optional: Instantiate and configure the drag icon
        if (dragIcon == null)
        {
            UIDraggedItem dragIconPrefab = InventoryManager.instance.uIDraggedItem;
            dragIcon = dragIconPrefab;
            dragIcon.gameObject.SetActive(true);
            dragIcon.SetItem(currentItem);
        }

        if (dragIcon != null)
        {
            // Update the position of the drag icon
            dragIcon.transform.position = Input.mousePosition;
        }

        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentItem.itemData == null)
            return;

        canvasGroup.blocksRaycasts = true;

        // Check if the item is being dragged onto a drop slot
        if (eventData.pointerEnter != null)
        {
            Debug.Log($"{currentItem.itemData.itemName} ON POINTER UP: {eventData.pointerEnter.name}");

            Btn_SlotItem droppedItemSlot = eventData.pointerEnter.GetComponent<Btn_SlotItem>();

            if (droppedItemSlot != null && droppedItemSlot != this)
            {
                // Implement your logic for handling the drop (e.g., swapping items)
                // You may want to communicate with your inventory management system
                Debug.Log("Item dropped onto " + droppedItemSlot.name);

                InventoryManager.instance.SwitchItemSlot(currentItem, droppedItemSlot);
            }
            else
            {
                // The drop is invalid, hide the drag icon and destroy it   
            }

            if (dragIcon != null)
            {
                dragIcon.gameObject.SetActive(false);
                dragIcon = null;
            }
        }
        else
        {
            Debug.Log("ON POINTER UP: B");

            // The drop is invalid, hide the drag icon and destroy it
            if (dragIcon != null)
            {
                dragIcon.gameObject.SetActive(false);
                dragIcon = null;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log($"{eventData.pointerDrag.gameObject.name} ON DROP to {this.gameObject.name}");
        // Optional: Implement additional drop logic if needed
    }
}
