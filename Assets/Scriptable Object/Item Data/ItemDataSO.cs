using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item Data", menuName ="Data/Item Data")]
public class ItemDataSO : ScriptableObject
{   
    public Sprite icon;
    public string itemName;
    [TextArea]
    public string desc;
    public GameObject prefabObject;
    public InteractionTypeSO interactionType;
}
