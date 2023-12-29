using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    InventoryManager inventoryManager;

    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] ThirdPersonController tpsController;
    [SerializeField] PlayerInteractionAbility[] interactions;

    ItemOwned currentItemInteraction;

    public bool isInteracting;
    public bool isUnableMove;


    private void Start()
    {
        inventoryManager = InventoryManager.instance;
    }

    public void StartInteraction(ItemOwned type)
    {
        currentItemInteraction = type;

        foreach (var item in interactions)
        {
            if(item.interactionTypeSO == type.itemData.interactionType)
            {
                isInteracting = true;
                item.DoInteraction(type.itemData.interactionType);
                break;
            }
        }
    }

    public void DoInteraction(bool isUnableMovement)
    {
        
        isUnableMove = isUnableMovement;


        if(isUnableMove)
        {
            DisableController();
        }
    }

    public void PeakInteraction()
    {
        if (currentItemInteraction.itemData.interactionType.isConsumeItem)
            ConsumeItem();

        playerAnimation.DestroyItemInHand();
    }

    public void FinishInteraction()
    {
        EnableController();


        

        isInteracting = false;

        currentItemInteraction = null;
        playerAnimation.OnQuickSlotItemSelected(currentItemInteraction);
    }

    void ConsumeItem()
    {
        inventoryManager.DecreaseItem(currentItemInteraction, 1);

        playerAnimation.DestroyItemInHand();
    }

    void DisableController()
    {
        tpsController.isAbleToJump = false;
        tpsController.isAbleToMove = false;
        tpsController.isAbleToRotate = false;
    }

    void EnableController()
    {
        tpsController.isAbleToJump = true;
        tpsController.isAbleToMove = true;
        tpsController.isAbleToRotate = true;
    }

}
