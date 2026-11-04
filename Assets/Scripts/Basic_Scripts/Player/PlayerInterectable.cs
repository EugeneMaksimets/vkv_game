using System.Collections.Generic;
using UnityEngine;

public class PlayerInterectable : MonoBehaviour
{
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
        if (_interactables.ContainsKey(other))
        {
            _interactables.Remove(other);
        }

    }

    private IInteractable GetClosestInteractable() {
        IInteractable closestInteractable = null;
        float closestDistanceSqr = float.MaxValue;

        foreach (KeyValuePair<Collider2D, IInteractable> entry in _interactables)
        {
            Collider2D collider = entry.Key;
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