using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIQuickSlot : MonoBehaviour
{
    QuickSlotManager quickSlotManager;

    public Btn_SlotItem[] allQuickSlots;

    public Action<ItemOwned> OnItemQuickSlotSelected;

    private void Start()
    {
        quickSlotManager = QuickSlotManager.instance;
        quickSlotManager.OnQuickSlotUpdated += OnUpdateQuickSlot;

        InitializeListenQuickSlotOnClicked();
    }

    void InitializeListenQuickSlotOnClicked()
    {
        for (int i = 0; i < allQuickSlots.Length; i++)
        {
            allQuickSlots[i].OnSelected += OnButtonSlotClicked;
        }
    }

    void OnButtonSlotClicked(ItemOwned itemOwn)
    {
        Debug.Log("LISTEN");
        OnItemQuickSlotSelected?.Invoke(itemOwn);
    }

    public void Initialize()
    {
        int unlockedSlotAmount = quickSlotManager.unlockedSlotAmount;

        for (int i = 0; i < allQuickSlots.Length; i++)
        {
            if (i <= unlockedSlotAmount)
            {
                allQuickSlots[i].gameObject.SetActive(true);
            }
            else
            {
                allQuickSlots[i].gameObject.SetActive(false);
            }
        }
    }

    void OnUpdateQuickSlot()
    {
        for (int i = 0; i < quickSlotManager.itemOnQuickSlots.Length; i++)
        {
            ItemOwned itemToAdd = quickSlotManager.itemOnQuickSlots[i];
            if(itemToAdd.itemData != null)
            {
                allQuickSlots[i].SetItem(quickSlotManager.itemOnQuickSlots[i]);
            }
            else
            {
                allQuickSlots[i].SetEmpty();
            }
        }

        //Debug.Log("UPDATE");
    }
}