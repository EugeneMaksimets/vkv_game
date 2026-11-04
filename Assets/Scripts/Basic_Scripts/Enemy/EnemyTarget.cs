using UnityEngine;

/// <summary>
/// Finds and stores the target for the enemy, typically the player. It searches for a GameObject with a specific tag and keeps a reference to its Transform.
/// </summary>
public class EnemyTarget : MonoBehaviour
{
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private float _searchInterval = 1f;

    public Transform TargetTransform { get; private set; }

    private float _nextSearchTime;

    private void Update()
    {
        if (TargetTransform != null || Time.time < _nextSearchTime)
            return;

        FindTarget();
    }

    public void SetTarget(Transform target)
    {
        TargetTransform = target;
    }

    public void ClearTarget()
    {
        TargetTransform = null;
    }

    private void FindTarget()
    {
        _nextSearchTime = Time.time + _searchInterval;

        GameObject targetObject = GameObject.FindWithTag(_targetTag);
        if (targetObject != null)
            TargetTransform = targetObject.transform;
    }

    private void OnValidate()
    {
        _searchInterval = Mathf.Max(0.1f, _searchInterval);
    }
}
