using UnityEngine;
using UnityEngine.UI;

public class UIPick : MonoBehaviour
{
    [SerializeField] GameObject btnPick;
    [SerializeField] Image imgPickableIcon;

    PickableItem currentPickableItem;
    public void ShowItemToPick(PickableItem item)
    {
        // Implement your logic to show the UI for the specific PickableItem
        //Debug.Log("Showing UI for " + item.name);

        btnPick.gameObject.SetActive(true);
        imgPickableIcon.sprite = item.itemData.icon;

        currentPickableItem = item;
    }

    public void HideItemToPick()
    {
        // Implement your logic to hide the UI
        //Debug.Log("Hiding UI");

        btnPick.gameObject.SetActive(false);
    }

    public void PickItem()
    {
        currentPickableItem.Pickup();
        HideItemToPick();
    }
}
