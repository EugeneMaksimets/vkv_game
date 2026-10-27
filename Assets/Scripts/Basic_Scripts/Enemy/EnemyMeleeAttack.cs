using System;
using UnityEngine;

/// <summary>
/// Выполняет удар по цели EnemyTarget, когда она находится в радиусе атаки.
/// </summary>
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyTarget))]
public class EnemyMeleeAttack : MonoBehaviour
{
    [Header ("Knockback")]
    [SerializeField] private float _knockbackSpeed = 4f;
    [SerializeField] private float _knockbackDuraction = 0.12f;

    [Header("DamageInfo")]
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _attackRange = 1.2f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private bool _attackAutomatically = true;

    public event Action<Transform> AttackStarted;
    public event Action<IDamageable, DamageInfo> DamageDealt;

    public bool IsTargetInRange
    {
        get
        {
            if (_target == null || _target.Target == null)
                return false;

            float rangeSquared = _attackRange * _attackRange;
            return ((Vector2)(_target.Target.position - transform.position)).sqrMagnitude <= rangeSquared;
        }
    }

    public bool CanAttackNow =>
        !_health.IsDead &&
        IsTargetInRange &&
        Time.time >= _nextAttackTime;

    private EnemyHealth _health;
    private EnemyTarget _target;
    private float _nextAttackTime;

    private void Awake()
    {
        _health = GetComponent<EnemyHealth>();
        _target = GetComponent<EnemyTarget>();
    }

    private void OnEnable()
    {
        _health.Died += DisableAttack;
    }

    private void OnDisable()
    {
        _health.Died -= DisableAttack;
    }

    private void Update()
    {
        if (_attackAutomatically)
            TryAttack();
    }

    public void SetAutomaticAttack(bool value)
    {
        _attackAutomatically = value;
    }

    public bool TryAttack()
    {
        if (!CanAttackNow)
            return false;

        IDamageable damageable = DamageableResolver.Find(_target.Target);
        if (damageable == null)
            return false;

        DamageInfo damageInfo = new DamageInfo(
            _damage,
            (Vector2)transform.position,
            _knockbackSpeed,
            _knockbackDuraction
        );

        AttackStarted?.Invoke(_target.Target);
        damageable.TakeDamage(damageInfo);
        DamageDealt?.Invoke(damageable, damageInfo);

        _nextAttackTime = Time.time + _attackCooldown;
        return true;
    }

    private void DisableAttack(EnemyHealth health)
    {
        enabled = false;
    }

    private void OnValidate()
    {
        _damage = Mathf.Max(0f, _damage);
        _attackRange = Mathf.Max(0f, _attackRange);
        _attackCooldown = Mathf.Max(0.01f, _attackCooldown);
        _knockbackSpeed = Mathf.Max(0f, _knockbackSpeed);
        _knockbackDuraction = Mathf.Max(0f, _knockbackDuraction);
    }
}
