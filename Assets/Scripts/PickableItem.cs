using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickable
{
    public ItemDataSO itemData;

    public void Pickup()
    {
        InventoryManager.instance.AddItem(itemData);
        Destroy(gameObject);
    }

    
}