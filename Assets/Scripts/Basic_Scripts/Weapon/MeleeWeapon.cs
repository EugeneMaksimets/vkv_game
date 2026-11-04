using UnityEngine;
using System;
using System.Collections.Generic;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float _damageAmount = 1f;

    [Header("Knockback")]
    [SerializeField] private float _knockbackSpeed = 8f;
    [SerializeField] private float _knockbackDuration = 10f; 

    private readonly HashSet<Collider2D> _targetsInRange = new();

    public event Action<IDamageable, DamageInfo> DamageDealt;

    void OnTriggerEnter2D(Collider2D other)
    {
        _targetsInRange.Add(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        _targetsInRange.Remove(other);
    }


    public bool Attack(Transform target, Vector2 attackOrigin)
    {
        if (target == null)
            return false;

        if (!IsTargetInRange(target))
            return false;

        IDamageable damageable = DamageableResolver.Find(target);

        if (damageable == null)
            return false;

        DamageInfo damageInfo = new DamageInfo(
            _damageAmount,
            attackOrigin,
            _knockbackSpeed,
            _knockbackDuration
        );

        damageable.TakeDamage(damageInfo);

        DamageDealt?.Invoke(damageable, damageInfo);

        return true;
    }

    private bool IsTargetInRange(Transform target)
    {
        if (target == null)
            return false;

        foreach (Collider2D collider in _targetsInRange)
        {
            if (collider == null)
                continue;

            if (collider.transform == target ||
                collider.transform.IsChildOf(target))
            {
                return true;
            }
        }

        return false;
    }

    private void OnValidate()
    {
        _damageAmount = Mathf.Max(0f, _damageAmount);
        _knockbackSpeed = Mathf.Max(0f, _knockbackSpeed);
        _knockbackDuration = Mathf.Max(0f, _knockbackDuration);
    }

    private void OnDisable()
    {
        _targetsInRange.Clear();
    }

}
