using System;
using UnityEngine;

public class EnemyMelee : Enemy
{
    [Header("Attack Settings")]
    [SerializeField] private float _attackRange = 2.5f;
    [SerializeField] private float _attackCooldown = 1f;

    [Header("Weapon")]
    [SerializeField] private MeleeWeapon _meleeWeapon;

    public event Action<Transform> AttackStarted;

    private float _nextAttackTime;

    protected override void Awake()
    {
        base.Awake();

        if(_meleeWeapon == null)
        {
            _meleeWeapon = GetComponentInChildren<MeleeWeapon>(true);
        }
        
        if (_meleeWeapon == null)
        {
            Debug.LogError("MeleeWeapon not found!", this);
            enabled = false;
            return;
        }
            
    }

    public override void TryAttack()
    {
        if (!CanAttackNow())
            return;

        Transform targetTransform = Target.TargetTransform;

        bool attackSucceeded = _meleeWeapon.Attack(
            targetTransform,
            transform.position);

        if (!attackSucceeded)
            return;

        AttackStarted?.Invoke(targetTransform);

        _nextAttackTime = Time.time + _attackCooldown;
    }

    public override bool IsTargetInRange()
    {
        Transform targetTransform = Target.TargetTransform;

        if (targetTransform == null)
            return false;

        float rangeSquared = _attackRange * _attackRange;

        Vector2 offset = targetTransform.position - transform.position;

        return offset.sqrMagnitude <= rangeSquared;
    }

    public override bool CanAttackNow()
    {
        return !Health.IsDead
               && IsTargetInRange()
               && Time.time >= _nextAttackTime;
    }

    private void OnValidate()
    {
        _attackRange = Mathf.Max(0f, _attackRange);
        _attackCooldown = Mathf.Max(0.01f, _attackCooldown);
    }
}
