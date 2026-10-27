using UnityEngine;

public class PlayerInterectable : MonoBehaviour
{
    public IInteractable CurrentInteractable { get; private set; }
    public string CurrentTag { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            CurrentInteractable = interactable;
            CurrentTag = other.tag;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            if (CurrentInteractable == interactable)
            {
                CurrentInteractable = null;
                CurrentTag = null;
            }
        }
    }
}