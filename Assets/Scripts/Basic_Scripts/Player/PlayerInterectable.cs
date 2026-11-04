using System.Collections.Generic;
using UnityEngine;

public class PlayerInterectable : MonoBehaviour
{
    // Tracks all interactable colliders currently inside the player's interaction range.
    private readonly Dictionary<Collider2D, IInteractable> _interactables = 
        new Dictionary<Collider2D, IInteractable>();
    public IInteractable CurrentInteractable => GetClosestInteractable();
    public string CurrentTag => (CurrentInteractable as MonoBehaviour)?.tag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            _interactables[other] = interactable;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _interactables.Remove(other);
    }

    // Returns the closest interactable to ensure the player interacts with the most relevant object.
    private IInteractable GetClosestInteractable() {
        IInteractable closestInteractable = null;
        float closestDistanceSqr = float.MaxValue;

        foreach (KeyValuePair<Collider2D, IInteractable> entry in _interactables)
        {
            Collider2D collider = entry.Key;
            // Use the closest point on the collider instead of its center.
            Vector2 closestPoint = collider.ClosestPoint(transform.position);

            float distanceSqr = 
                ((Vector2)transform.position - closestPoint).sqrMagnitude;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestInteractable = entry.Value;
            }
        }
        return closestInteractable;
    }
}