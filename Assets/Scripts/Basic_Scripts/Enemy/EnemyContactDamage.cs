using System;
using UnityEngine;

/// <summary>
/// Наносит урон цели с тегом Player, пока та касается trigger-коллайдера врага.
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyContactDamage : MonoBehaviour
{
    [Header("Knockback")]
    [SerializeField] private float _knockbackSpeed = 4f;
    [SerializeField] private float _knockbackDuration = 0.12f;

    [Header("DamageInfo")]
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _damageInterval = 1f;
    [SerializeField] private string _targetTag = "Player";

    public event Action<IDamageable, DamageInfo> DamageDealt;

    private EnemyHealth _health;
    private float _nextDamageTime;

    private void Awake()
    {
        _health = GetComponent<EnemyHealth>();
    }

    private void OnEnable()
    {
        _health.Died += DisableDamage;
    }

    private void OnDisable()
    {
        _health.Died -= DisableDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDealDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDealDamage(other);
    }

    private void TryDealDamage(Collider2D other)
    {
        if (Time.time < _nextDamageTime ||
            !DamageableResolver.HasTagInParents(other.transform, _targetTag))
            return;

        IDamageable target = DamageableResolver.Find(other.transform);
        if (target == null)
            return;

        DamageInfo damageInfo = new DamageInfo(
            _damage,
            (Vector2)transform.position,
            _knockbackSpeed,
            _knockbackDuration
        );
        target.TakeDamage(damageInfo);
        DamageDealt?.Invoke(target, damageInfo);

        _nextDamageTime = Time.time + _damageInterval;
    }

    private void DisableDamage(EnemyHealth health)
    {
        enabled = false;
    }

    private void OnValidate()
    {
        _damage = Mathf.Max(0f, _damage);
        _damageInterval = Mathf.Max(0.01f, _damageInterval);
        _knockbackSpeed = Mathf.Max(0f, _knockbackSpeed);
        _knockbackDuration = Mathf.Max(0f, _knockbackDuration);
    }
}
