using UnityEngine;
using System;
using System.Collections.Generic;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float _damageAmount = 1f;

    [Header("Knockback")]
    [SerializeField] private float _knockbackSpeed = 3f;
    [SerializeField] private float _knockbackDuration = 1f; 

    [Header("Filtering")]
    [SerializeField] private bool _ignoreTriggers = true;

    private readonly HashSet<Collider2D> _targetsInRange = new();

    public event Action<IDamageable, DamageInfo> DamageDealt;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
            return;

        if (_ignoreTriggers && other.isTrigger)
            return;

        if (DamageableResolver.Find(other.transform) != null)
        {
            _targetsInRange.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == null)
            return;

        _targetsInRange.Remove(other);
    }

    public bool CanHit(Transform target)
    {
        if (target == null)
            return false;

        _targetsInRange.RemoveWhere(c => c == null);

        foreach (Collider2D collider in _targetsInRange)
        {
            if (collider.transform == target ||
                collider.transform.IsChildOf(target))
            {
                return true;
            }
        }

        return false;
    }

    public bool Attack(Transform target, Vector2 attackOrigin)
    {
        if (!CanHit(target))
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
