using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInteractSelectedItem : MonoBehaviour
{
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] UIQuickSlot uIQuickSlot;

    [SerializeField] GameObject btnInteract;
    [SerializeField] TextMeshProUGUI textInteract;
    [SerializeField] Image imgIconButton;

    ItemOwned currentItem;


    private void Start()
    {
        uIQuickSlot.OnItemQuickSlotSelected += ShowInteractUI;
    }


    public void ShowInteractUI(ItemOwned itemOwn)
    {
        if (itemOwn != null)
        {
            if (itemOwn.itemData != null)
            {
                btnInteract.gameObject.SetActive(true);
                imgIconButton.sprite = itemOwn.itemData.icon;

                currentItem = itemOwn;
            }
            else
            {
                HideInteractUI();
            }
        }
        else
        {
            HideInteractUI();
        }
    }


    public void HideInteractUI()
    {
        btnInteract.gameObject.SetActive(false);
    }

    public void DoInteract()
    {
        if(!playerInteraction.isInteracting)
        {
            playerInteraction.StartInteraction(currentItem);

            HideInteractUI();
        }
    }
}