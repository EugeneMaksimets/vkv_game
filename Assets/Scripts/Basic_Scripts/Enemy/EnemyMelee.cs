using System;
using UnityEngine;

public class EnemyMelee : Enemy
{
    [Header("Attack Settings")]
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
        if (Health.IsDead)
            return;

        _nextAttackTime = Time.time + _attackCooldown;

        Transform targetTransform = Target.TargetTransform;

        bool attackSucceeded = _meleeWeapon.Attack(
            targetTransform,
            transform.position);

        if (!attackSucceeded)
            return;

        AttackStarted?.Invoke(targetTransform);

    }

    public override bool IsTargetInRange()
    {
        return _meleeWeapon.CanHit(Target.TargetTransform);
    }

    public override bool CanAttackNow()
    {
        return !Health.IsDead
               && IsTargetInRange()
               && Time.time >= _nextAttackTime;
    }

    private void OnValidate()
    {
        _attackCooldown = Mathf.Max(0.01f, _attackCooldown);
    }
}
