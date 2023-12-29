using UnityEngine;

public class PlayerPickAbility : MonoBehaviour
{
    public float pickRadius = 2f;
    public LayerMask pickableLayer; // Set this to the layer where your pickable objects belong
    public float checkInterval = 0.3f; // Time interval for detection
    private float timeSinceLastCheck = 0f;
    [SerializeField] UIPick uiPick;
    private PickableItem nearestPickableItem;


    void Update()
    {
        // Update the time since the last check
        timeSinceLastCheck += Time.deltaTime;

        // Check for pickable objects at a specified interval
        if (timeSinceLastCheck >= checkInterval)
        {
            // Detect pickable objects within the specified radius
            nearestPickableItem = FindNearestPickable();

            // If there is a pickable object in range, show UI to pick
            if (nearestPickableItem != null)
            {
                uiPick.ShowItemToPick(nearestPickableItem);
            }
            else
            {
                uiPick.HideItemToPick(); // You might want to implement a function to hide the UI as well
            }

            // Reset the time since the last check
            timeSinceLastCheck = 0f;
        }

        //if(Input.GetKeyDown(KeyCode.E)) //Debug PICK
        //{
        //    //PICK
        //    FindObjectOfType<UIPick>().PickItem();
        //}
    }

    // Visualize the pick radius in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickRadius);
    }

    PickableItem FindNearestPickable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickRadius, pickableLayer);
        PickableItem nearestPickable = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var collider in hitColliders)
        {
            PickableItem pickableObject = collider.GetComponent<PickableItem>();

            if (pickableObject != null)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPickable = pickableObject;
                }
            }
        }

        return nearestPickable;
    }
}
