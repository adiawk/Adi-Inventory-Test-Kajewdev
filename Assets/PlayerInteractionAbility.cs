using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionAbility : MonoBehaviour
{
    public InteractionTypeSO interactionTypeSO;
    public PlayerInteraction playerInteraction;
    public Animator anim;

    InteractionTypeSO currentInteraction;
    public virtual void DoInteraction(InteractionTypeSO interactionType)
    {
        playerInteraction.DoInteraction(interactionType.disableAllLocomotive);

        currentInteraction = interactionType;
        PlayAnimation();

        StartCoroutine(Interacting());
    }

    protected virtual void PlayAnimation()
    {
        anim.SetTrigger(currentInteraction.animationTrigger);
    }

    IEnumerator Interacting()
    {
        yield return new WaitForSeconds(currentInteraction.peakInteractionTime);
        playerInteraction.PeakInteraction();

        yield return new WaitForSeconds(currentInteraction.interactionTime - currentInteraction.peakInteractionTime);
        FinishInteraction();
    }

    public virtual void FinishInteraction()
    {
        playerInteraction.FinishInteraction();
    }
}
