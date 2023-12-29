using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDraggedItem : MonoBehaviour
{
    [SerializeField] Image imgIcon;
    [SerializeField] TextMeshProUGUI txtAmount;

    public void SetItem(ItemOwned itemOwn)
    {
        imgIcon.sprite = itemOwn.itemData.icon;
        txtAmount.text = itemOwn.amount.ToString();
    }
}
