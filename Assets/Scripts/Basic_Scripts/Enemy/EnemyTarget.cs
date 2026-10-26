using UnityEngine;

/// <summary>
/// Находит и хранит текущую цель врага. По умолчанию это объект с тегом Player.
/// </summary>
public class EnemyTarget : MonoBehaviour
{
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private float _searchInterval = 1f;

    public Transform Target { get; private set; }

    private float _nextSearchTime;

    private void Update()
    {
        if (Target != null || Time.time < _nextSearchTime)
            return;

        FindTarget();
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    public void ClearTarget()
    {
        Target = null;
    }

    private void FindTarget()
    {
        _nextSearchTime = Time.time + _searchInterval;

        GameObject targetObject = GameObject.FindWithTag(_targetTag);
        if (targetObject != null)
            Target = targetObject.transform;
    }

    private void OnValidate()
    {
        _searchInterval = Mathf.Max(0.1f, _searchInterval);
    }
}
