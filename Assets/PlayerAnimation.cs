using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] UIQuickSlot uiQuickSlot;

    [SerializeField] Animator anim;

    [SerializeField] Transform itemCarrySpawnPoint;

    GameObject currentCarriedItem;


    // Start is called before the first frame update
    void Start()
    {
        uiQuickSlot.OnItemQuickSlotSelected += OnQuickSlotItemSelected;
    }

    public void OnQuickSlotItemSelected(ItemOwned itemOwn)
    {
        //if(itemOwn == null)
        //{
        //    ResetOnHandItem();
        //    anim.SetBool("IsCarrying", false);

        //    return;
        //}

        try
        {
            ResetOnHandItem();
            anim.SetBool("IsCarrying", true);

            currentCarriedItem = Instantiate(itemOwn.itemData.prefabObject, itemCarrySpawnPoint);
        }
        catch
        {
            ResetOnHandItem();
            anim.SetBool("IsCarrying", false);
        }
    }

    void ResetOnHandItem()
    {
        if (currentCarriedItem != null)
        {
            DestroyItemInHand();
        }
        else
        {
            currentCarriedItem = null;
        }
    }

    public void DestroyItemInHand()
    {
        try
        {
            Destroy(currentCarriedItem.gameObject);
        }
        catch { };

        currentCarriedItem = null;
    }
}
