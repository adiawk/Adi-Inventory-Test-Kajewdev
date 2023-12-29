using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory_QuickSlot : MonoBehaviour
{
    QuickSlotManager quickSlotManager;

    [SerializeField] UI_ItemInfo uiItemInfo;

    public Btn_SlotItem[] allQuickSlots;


    private void OnEnable()
    {
        if(quickSlotManager == null)
            quickSlotManager = QuickSlotManager.instance;

        InitializeSlot();
        UpdateItem();
    }

    private void Start()
    {
        quickSlotManager.OnQuickSlotUpdated += UpdateItem;

        for (int i = 0; i < allQuickSlots.Length; i++)
        {
            allQuickSlots[i].isQuickSlot = true;
            allQuickSlots[i].slotIndex = i;
            allQuickSlots[i].OnSelected += OnButtonSlotClicked;
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

    void InitializeSlot()
    {
        int unlockedSlotAmount = quickSlotManager.unlockedSlotAmount;

        for (int i = 0; i < allQuickSlots.Length; i++)
        {
            if (i <= unlockedSlotAmount)
            {
                allQuickSlots[i].SetUnlocked();
            }
            else
            {
                allQuickSlots[i].SetLocked();
            }
        }
    }

    //Update if only new item drop to this slot
    void UpdateItem()
    {
        for (int i = 0; i < quickSlotManager.itemOnQuickSlots.Length; i++)
        {
            ItemOwned itemToAdd = quickSlotManager.itemOnQuickSlots[i];
            if (itemToAdd.itemData != null)
            {
                allQuickSlots[i].SetItem(quickSlotManager.itemOnQuickSlots[i]);
            }
            else
            {
                allQuickSlots[i].SetEmpty();
            }
        }
    }

    
}