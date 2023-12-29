using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory_AllSlot : MonoBehaviour
{
    InventoryManager inventoryManager;
    [SerializeField] UI_ItemInfo uiItemInfo;

    public Btn_SlotItem[] allSlots;


    private void OnEnable()
    {
        if(inventoryManager == null)
            inventoryManager = InventoryManager.instance;

        UpdateSlotitem();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeSlot();

        inventoryManager.OnAnyItemChanged += UpdateSlotitem;
    }

    void InitializeSlot()
    {
        for (int i = 0; i < allSlots.Length; i++)
        {
            allSlots[i].slotIndex = i;
            allSlots[i].isQuickSlot = false;
            allSlots[i].OnSelected += OnButtonSlotClicked;

        }
    }

    void OnButtonSlotClicked(ItemOwned itemOwn)
    {
        try
        {
            uiItemInfo.SetPreview(itemOwn.itemData);
        }
        catch
        {
            uiItemInfo.SetPreviewEmpty();
        };
    }

    void UpdateSlotitem()
    {
        for (int i = 0; i < allSlots.Length; i++)
        {
            if (inventoryManager.itemSlots[i].itemData != null)
            {
                allSlots[i].SetItem(inventoryManager.itemSlots[i]);
            }
            else
            {
                allSlots[i].SetEmpty();
            }
        }
    }
}
