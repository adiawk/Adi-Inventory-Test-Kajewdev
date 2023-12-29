using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemInfo : MonoBehaviour
{
    [SerializeField] Image imgIcon;
    [SerializeField] TextMeshProUGUI textItemName;
    [SerializeField] TextMeshProUGUI textItemDesc;

    
    public void SetPreview(ItemDataSO data)
    {
        try
        {
            imgIcon.sprite = data.icon;
        }
        catch { };
        
        textItemName.text = data.itemName;
        textItemDesc.text = data.desc;
    }

    public void SetPreviewEmpty()
    {
        imgIcon.sprite = null;
        textItemName.text = "";
        textItemDesc.text = "";
    }
}
