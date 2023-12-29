using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    public static QuickSlotManager instance;

    [SerializeField] UIQuickSlot UIQuickSlot;
    public int unlockedSlotAmount = 2;

    public ItemOwned[] itemOnQuickSlots = new ItemOwned[7];

    public Action OnQuickSlotUpdated;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeSlot();
    }

    void InitializeSlot()
    {
        UIQuickSlot.Initialize();
    }

    public void UpdateQuickSlot()
    {
        OnQuickSlotUpdated?.Invoke();

        Debug.Log("Quick Slot UI Updated");
    }

    public void AutoAddItemToQuickSlot(ItemOwned itemOwn, out bool isSuccess)
    {
        //Check if there is already that item in the quick slot
        foreach (var item in itemOnQuickSlots)
        {
            if(item.itemData == itemOwn.itemData)
            {
                isSuccess = true;
                item.amount += itemOwn.amount;
                return;
            }
        }
        
        //Check if slot is avaiable
        for (int i = 0; i < itemOnQuickSlots.Length; i++)
        {
            if (IsSlotUnlock(i))
            {
                if (IsSlotEmpty(i))
                {
                    AddItemToEmptySlot(itemOwn, i);
                    isSuccess = true;
                    return;
                }
            }
        }
        isSuccess = false;
    }

    void AddItemToEmptySlot(ItemOwned itemOwn, int targetSlot)
    {
        itemOnQuickSlots[targetSlot] = itemOwn;
        itemOwn.isInQuickSlot = true;
        itemOwn.slotIndex = targetSlot;

        //Debug.Log("ADD ITEM TO QUICK SLOT");
    }

    bool IsSlotUnlock(int targetSlot)
    {
        return targetSlot <= unlockedSlotAmount;
    }

    bool IsSlotEmpty(int targetSlot)
    {
        return itemOnQuickSlots[targetSlot].itemData == null;
    }


}
