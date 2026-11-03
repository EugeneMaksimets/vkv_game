using System;
using UnityEngine;

/// <summary>
/// Выполняет удар по цели EnemyTarget, когда она находится в радиусе атаки.
/// </summary>
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyTarget))]
public class EnemyMelee : Enemy
{

    [Header("Attack Settings")]
    [SerializeField] private float _attackRange = 1.2f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private bool _attackAutomatically = true;

    public event Action<Transform> AttackStarted;

    private MeleeWeapon _meleeWeapon;
    private float _nextAttackTime;

    protected override void Awake()
    {
        base.Awake();

        _meleeWeapon = GetComponent<MeleeWeapon>();
    }

    private void OnEnable()
    {
        Health.Died += DisableAttack;
    }

    private void OnDisable()
    {
        Health.Died -= DisableAttack;
    }

    private void Update()
    {
        if (_attackAutomatically)
            TryAttack();
    }

    override public void SetAutomaticAttack(bool value)
    {
        _attackAutomatically = value;
    }

    override public void TryAttack()
    {
        if (!CanAttackNow())
            return;

        Transform targetTransform = Target.Target;
        AttackStarted?.Invoke(targetTransform);

        if (_meleeWeapon.IsWeaponReady(targetTransform, targetTransform.position))
        {
            _meleeWeapon.Attack(targetTransform);
        }
        _nextAttackTime = Time.time + _attackCooldown;

    }

    private void DisableAttack(EnemyHealth health)
    {
        enabled = false;
    }

    private void OnValidate()
    {
        _attackRange = Mathf.Max(0f, _attackRange);
        _attackCooldown = Mathf.Max(0.01f, _attackCooldown);
    }

    override public bool IsTargetInRange()
    {
        
        if (Target == null || Target.Target == null)
            return false;

        float rangeSquared = _attackRange * _attackRange;
        return ((Vector2)(Target.Target.position - transform.position)).sqrMagnitude <= rangeSquared;
        
    }

    override public bool CanAttackNow()
    {
        return !Health.IsDead &&
               IsTargetInRange() &&
               Time.time >= _nextAttackTime;
    }

}
