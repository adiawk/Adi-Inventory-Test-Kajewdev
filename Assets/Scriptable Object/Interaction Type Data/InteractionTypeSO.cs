using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="InteractionTypeSO", menuName = "Data/Interaction Type Data")]
public class InteractionTypeSO : ScriptableObject
{
    public bool disableAllLocomotive;
    public string animationTrigger;
    public float peakInteractionTime;
    public float interactionTime;
    public bool isConsumeItem;
}
