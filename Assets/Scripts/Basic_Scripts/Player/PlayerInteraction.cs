using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Cleanup")]
    // Timer for clean interactable object
    [SerializeField, Min(0.1f)] private float _cleanupInterval = 0.2f;
    private float _cleanupTimer;
    // Tracks all interactable Colliders currently inside the player's interaction range.
    private readonly Dictionary<IInteractable, HashSet<Collider2D>> _interactables = new();
    private readonly List<IInteractable> _invalidInteractables = new();
    private readonly List<Collider2D> _invalidColliders = new();
    private IInteractable _currentInteractable;
    public IInteractable CurrentInteractable => _currentInteractable;
    public bool HasInteractable =>
    CurrentInteractable is UnityEngine.Object unityObject && unityObject != null;
    public string CurrentTag => (CurrentInteractable as MonoBehaviour)?.tag;


    private void Update()
    {
        if(!UpdateCleanupTimer())
            return;
        
        CleanupInvalidInteractables();
        _currentInteractable = GetClosestInteractable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IInteractable>(out var interactable))
        {
            return;
        }

        if (!_interactables.TryGetValue(interactable, out var colliders))
        {
            colliders = new HashSet<Collider2D>();
            _interactables.Add(interactable, colliders);
        }

        colliders.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<IInteractable>(out var interactable))
        {
            return;
        }

        if(!_interactables.TryGetValue(interactable, out var colliders))
        {
            return;
        }

        colliders.Remove(other);

        if(colliders.Count == 0)
        {
            _interactables.Remove(interactable);
        }
    }

    // Returns the closest interactable to ensure the player interacts with the most relevant object.
    private IInteractable GetClosestInteractable()
    {
        IInteractable closestInteractable = null;
        float closestDistance = float.MaxValue;

        Vector2 playerPosition = transform.position;

        foreach (var pair in _interactables)
        {
            IInteractable interactable = pair.Key;
            HashSet<Collider2D> colliders = pair.Value;
            if ((interactable as UnityEngine.Object) == null)
                continue;

            foreach (Collider2D collider in colliders)
            {
                if (collider == null)
                {
                    continue;
                }

                Vector2 closestPoint = collider.ClosestPoint(playerPosition);
                float distance = (playerPosition - closestPoint).sqrMagnitude;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }
    
    private void CleanupInvalidInteractables()
    {
        _invalidInteractables.Clear();

        foreach (var pair in _interactables)
        {
            IInteractable interactable = pair.Key;
            HashSet<Collider2D> colliders = pair.Value;
            // IInteractable is a interface need cast to object 
            if ((interactable as UnityEngine.Object) == null)
            {
                _invalidInteractables.Add(interactable);
                continue;
            }

            _invalidColliders.Clear();

            foreach (Collider2D collider in colliders)
            {
                if (collider == null)
                {
                    _invalidColliders.Add(collider);
                }
            }

            foreach (Collider2D invalidCollider in _invalidColliders)
            {
                colliders.Remove(invalidCollider);
            }

            if (colliders.Count == 0)
            {
                _invalidInteractables.Add(interactable);
            }
        }

        foreach (IInteractable interactable in _invalidInteractables)
        {
            _interactables.Remove(interactable);
        }
        if (_currentInteractable != null &&
            !_interactables.ContainsKey(_currentInteractable))
        {   
            _currentInteractable = null;
        }   
    }
    private bool UpdateCleanupTimer()
    {
        _cleanupTimer += Time.deltaTime;

        if (_cleanupTimer < _cleanupInterval)
        {
            return false;
        }

        _cleanupTimer = 0f;
        return true;
    }

}