using System;
using UnityEngine;

/// <summary>
/// Atacking enemy that uses a melee weapon to damage the player. It checks if the player is within attack range and manages attack cooldowns.
/// </summary>

[RequireComponent(typeof(MeleeWeapon))]
public class EnemyMelee : Enemy
{

    [Header("Attack Settings")]
    [SerializeField] private float _attackRange = 2.5f;
    [SerializeField] private float _attackCooldown = 1f;

    public event Action<Transform> AttackStarted;

    private MeleeWeapon _meleeWeapon;
    private float _nextAttackTime;

    protected override void Awake()
    {
        base.Awake();

        _meleeWeapon = GetComponent<MeleeWeapon>();
    }
    override public void TryAttack()
    {
        if (!CanAttackNow())
            return;

        Transform targetTransform = Target.TargetTransform;

        AttackStarted?.Invoke(targetTransform);

        bool attackSucceeded = _meleeWeapon.Attack(
        targetTransform,
        transform.position
        );

        if (!attackSucceeded)
            return;

        _nextAttackTime = Time.time + _attackCooldown;

    }

    private void OnValidate()
    {
        _attackRange = Mathf.Max(0f, _attackRange);
        _attackCooldown = Mathf.Max(0.01f, _attackCooldown);
    }

    override public bool IsTargetInRange()
    {
        
        if (Target == null || Target.TargetTransform == null)
            return false;

        float rangeSquared = _attackRange * _attackRange;
        return ((Vector2)(Target.TargetTransform.position - transform.position)).sqrMagnitude <= rangeSquared;
        
    }

    override public bool CanAttackNow()
    {
        return !Health.IsDead &&
               IsTargetInRange() &&
               Time.time >= _nextAttackTime;
    }

}
