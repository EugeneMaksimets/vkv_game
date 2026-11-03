using UnityEngine;
using System;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float _damageAmount = 1f;

    [Header("Knockback")]
    [SerializeField] private float _knockbackSpeed = 8f;
    [SerializeField] private float _knockbackDuration = 10f; 


    public event Action<IDamageable, DamageInfo> DamageDealt;


    public bool Attack(Transform target, Vector2 attackOrigin)
    {
        if (target == null)
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

}
