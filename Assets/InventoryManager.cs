using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemOwned
{
    public ItemDataSO itemData;
    public int amount;
    public bool isInQuickSlot;
    public int slotIndex;

    public void AddItem(int amount = 1)
    {
        this.amount += amount;
    }

    public void DecreaseItem(int amount =1)
    {
        this.amount -= amount;
    }
}

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance;
    
    public UIDraggedItem uIDraggedItem;

    [SerializeField] QuickSlotManager quickSlotManager;
    [SerializeField] UIInventory_AllSlot uIInventory_AllSlot;

    public ItemOwned[] itemSlots = new ItemOwned[32];

    public Action OnAnyItemChanged;

    private void Awake()
    {
        instance = this;
    }

    public void AddItem(ItemDataSO data, int amount = 1)
    {
        foreach (var item in itemSlots)
        {
            if (item.itemData == data)
            {
                item.AddItem(amount);
                quickSlotManager.UpdateQuickSlot();
                return;
            }
        }

        
        int availableSlot = 0;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].itemData == null)
            {
                availableSlot = i;
                break;
            }
        }

        ItemOwned addedItem = new ItemOwned
        {
            itemData = data,
            amount = amount
        };

        itemSlots[availableSlot] = addedItem;

        quickSlotManager.AutoAddItemToQuickSlot(addedItem, out bool isSucceddAddToQuickSlot);

        if (isSucceddAddToQuickSlot)
        {
            //remove from all inventory slot
            itemSlots[availableSlot] = new ItemOwned { itemData = null, amount = 0 };

            Debug.Log("REMOVE from ALL SLOT");
        }

        quickSlotManager.UpdateQuickSlot();
        OnAnyItemChanged?.Invoke();
    }

    public void DecreaseItem(ItemOwned decreasedItem, int amount = 1)
    {
        //Check dlu apakah ada itemnya
        bool isAnyItemHave = decreasedItem.amount >= amount;

        if(isAnyItemHave)
        {
            decreasedItem.amount -= amount;
        }

        if(decreasedItem.amount <= 0)
        {
            RemoveFromSlot(decreasedItem);
        }

        quickSlotManager.UpdateQuickSlot();
        OnAnyItemChanged?.Invoke();
    }

    void RemoveFromSlot(ItemOwned itemOwn)
    {
        if (itemOwn.isInQuickSlot)
        {
            quickSlotManager.itemOnQuickSlots[itemOwn.slotIndex] = new ItemOwned
            {
                itemData = null,
                amount = 0,
                isInQuickSlot = true,
                slotIndex = itemOwn.slotIndex
            };
        }
        else
        {
            itemSlots[itemOwn.slotIndex] = new ItemOwned
            {
                itemData = null,
                amount = 0,
                isInQuickSlot = false,
                slotIndex = itemOwn.slotIndex
            };
        }
    }
    
    public void SwitchItemSlot(ItemOwned dragItem, Btn_SlotItem indexTargetSlot)
    {
        //Apakah 'dragItem' ini dari quick slot atau inventory biasa
        bool ItemFromQuickSlot = dragItem.isInQuickSlot;

        //Apakah indexSlot yang dituju ini inventory atau quickslot
        bool SlotTargetIsQuickSlot = indexTargetSlot.isQuickSlot;

        //Apakan indexSlot tujuan kosong?
        bool SlotTargetEmpty = indexTargetSlot.currentItem.itemData == null;

        //Kalo isi, maka repplace slot index

        if (ItemFromQuickSlot) //Dragged item from QUICK SLOT
        {
            //Debug.Log("A: from quick slot");
            if (SlotTargetIsQuickSlot) //Target slot is QUICK SLOT
            {
                //Debug.Log("B: target is quick slot");
                if (SlotTargetEmpty)
                {
                    //Debug.Log("C: target is empty");

                    //Previous slot
                    int removedSlotIndex = dragItem.slotIndex;

                    //Add dragged item to new slot
                    quickSlotManager.itemOnQuickSlots[indexTargetSlot.slotIndex] = new ItemOwned
                    {
                        itemData = dragItem.itemData,
                        amount = dragItem.amount,
                        isInQuickSlot = true,
                        slotIndex = indexTargetSlot.slotIndex
                    };

                    //Remove previous slot
                    quickSlotManager.itemOnQuickSlots[removedSlotIndex] = new ItemOwned
                    {
                        itemData = null,
                        amount = 0
                    };
                }
                else //Switch Slot QUICK SLOT -> QUICK SLOT
                {
                    int sourceSlotIndex = dragItem.slotIndex;
                    int targetSlotIndex = indexTargetSlot.slotIndex;

                    //Temp target slot
                    ItemOwned tempTargetSlot = quickSlotManager.itemOnQuickSlots[targetSlotIndex];

                    //Set Source to Target
                    quickSlotManager.itemOnQuickSlots[targetSlotIndex] = new ItemOwned
                    {
                        itemData = dragItem.itemData,
                        amount = dragItem.amount,
                        isInQuickSlot = true,
                        slotIndex = targetSlotIndex
                    };

                    //Set Temp Target to Previous Source
                    quickSlotManager.itemOnQuickSlots[sourceSlotIndex] = new ItemOwned
                    {
                        itemData = tempTargetSlot.itemData,
                        amount = tempTargetSlot.amount,
                        isInQuickSlot = true,
                        slotIndex = sourceSlotIndex
                    };
                }
            }
            else //Target slot is INVENTORY
            {
                if (SlotTargetEmpty) //Is Inventory's Slot Empty
                {
                    int removedSlotIndex = dragItem.slotIndex;

                    //Add dragged item to empty INVENTORY Slot
                    itemSlots[indexTargetSlot.slotIndex] = new ItemOwned
                    {
                        itemData = dragItem.itemData,
                        amount = dragItem.amount,
                        isInQuickSlot = false,
                        slotIndex = indexTargetSlot.slotIndex
                    };

                    //Remove previous slot
                    quickSlotManager.itemOnQuickSlots[removedSlotIndex] = new ItemOwned
                    {
                        itemData = null,
                        amount = 0
                    };
                }
                else //Quick slot -> Inventory SWITCH slot
                {
                    int sourceSlotIndex = dragItem.slotIndex;
                    int targetSlotIndex = indexTargetSlot.slotIndex;

                    //Temp target slot
                    ItemOwned tempTargetSlot = itemSlots[targetSlotIndex];

                    //Set Source to Target
                    itemSlots[targetSlotIndex] = new ItemOwned
                    {
                        itemData = dragItem.itemData,
                        amount = dragItem.amount,
                        isInQuickSlot = false,
                        slotIndex = targetSlotIndex
                    };

                    //Set Temp Target to Previous Source
                    quickSlotManager.itemOnQuickSlots[sourceSlotIndex] = new ItemOwned
                    {
                        itemData = tempTargetSlot.itemData,
                        amount = tempTargetSlot.amount,
                        isInQuickSlot = true,
                        slotIndex = sourceSlotIndex
                    };
                }
            }
        }
        else //Dragged item from INVENTORY
        {
            if(SlotTargetIsQuickSlot)
            {
                if(SlotTargetEmpty)
                {
                    int removedSlotIndex = dragItem.slotIndex;

                    //Add Dragged Item from (INVENTORY) to new slot (QUICK SLOT)
                    quickSlotManager.itemOnQuickSlots[indexTargetSlot.slotIndex] = new ItemOwned
                    {
                        itemData = dragItem.itemData,
                        amount = dragItem.amount,
                        isInQuickSlot = true,
                        slotIndex = indexTargetSlot.slotIndex
                    };

                    //Remove from previous Inventory Slot
                    itemSlots[removedSlotIndex] = new ItemOwned
                    {
                        itemData = null,
                        amount = 0,
                    };
                }
                else//Switch Slot INVENTORY -> QUICK SLOT
                {
                    int sourceSlotIndex = dragItem.slotIndex;
                    int targetSlotIndex = indexTargetSlot.slotIndex;

                    //Temp target slot
                    ItemOwned tempTargetSlot = quickSlotManager.itemOnQuickSlots[targetSlotIndex];

                    //Set Source to Target
                    quickSlotManager.itemOnQuickSlots[targetSlotIndex] = new ItemOwned
                    {
                        itemData = dragItem.itemData,
                        amount = dragItem.amount,
                        isInQuickSlot = true,
                        slotIndex = targetSlotIndex
                    };

                    //Set Temp Target to Previous Source
                    itemSlots[sourceSlotIndex] = new ItemOwned
                    {
                        itemData = tempTargetSlot.itemData,
                        amount = tempTargetSlot.amount,
                        isInQuickSlot = false,
                        slotIndex = sourceSlotIndex
                    };
                }
            }
            else //Dragged item from INVENTORY -> INVENTORY
            {
                if(SlotTargetEmpty) //Empty Inventory
                {
                    int removedSlotIndex = dragItem.slotIndex;

                    //Add Dragged Item from (INVENTORY) to new slot (QUICK SLOT)
                    itemSlots[indexTargetSlot.slotIndex] = new ItemOwned
                    {
                        itemData = dragItem.itemData,
                        amount = dragItem.amount,
                        isInQuickSlot = false,
                        slotIndex = indexTargetSlot.slotIndex
                    };

                    //Remove from previous Inventory Slot
                    itemSlots[removedSlotIndex] = new ItemOwned
                    {
                        itemData = null,
                        amount = 0,
                    };
                }
                else
                {
                    int sourceSlotIndex = dragItem.slotIndex;
                    int targetSlotIndex = indexTargetSlot.slotIndex;

                    //Temp target slot
                    ItemOwned tempTargetSlot = itemSlots[targetSlotIndex];

                    //Set Source to Target
                    itemSlots[targetSlotIndex] = new ItemOwned
                    {
                        itemData = dragItem.itemData,
                        amount = dragItem.amount,
                        isInQuickSlot = false,
                        slotIndex = targetSlotIndex
                    };

                    //Set Temp Target to Previous Source
                    itemSlots[sourceSlotIndex] = new ItemOwned
                    {
                        itemData = tempTargetSlot.itemData,
                        amount = tempTargetSlot.amount,
                        isInQuickSlot = false,
                        slotIndex = sourceSlotIndex
                    };
                }
            }
        }

        quickSlotManager.UpdateQuickSlot();
        OnAnyItemChanged?.Invoke();

    }

}