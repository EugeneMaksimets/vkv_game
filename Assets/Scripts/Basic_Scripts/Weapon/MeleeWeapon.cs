using UnityEngine;
using System;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float _damageAmount = 1f;

    [Header("Knockback")]
    [SerializeField] private float _knockbackSpeed = 4f;
    [SerializeField] private float _knockbackDuration = 0.12f; 

    private DamageInfo _damageInfo;
    private IDamageable _damageableTarget;

    public event Action<IDamageable, DamageInfo> DamageDealt;


     public bool IsWeaponReady(Transform target, Vector2 targetPosition)
    {
        if (target == null)
        {
            return false;   
        }

        _damageableTarget = DamageableResolver.Find(target);
        if (_damageableTarget == null) 
        {
            return false;
        }

        _damageInfo = new DamageInfo(
            _damageAmount,
            targetPosition,
            _knockbackSpeed,
            _knockbackDuration
        );

        return true;
    }

    public void Attack(Transform targetTransform)
    {
        _damageableTarget.TakeDamage(_damageInfo);
        DamageDealt?.Invoke(_damageableTarget, _damageInfo);
    }

}
