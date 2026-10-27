using UnityEngine;

public class PickUpedItem : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("PickUp Item");
    }
}
