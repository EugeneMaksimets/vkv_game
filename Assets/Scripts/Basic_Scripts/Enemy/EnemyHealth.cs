using System;
using UnityEngine;

/// <summary>
/// Хранит здоровье врага и сообщает остальным компонентам о получении урона и смерти.
/// </summary>
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 3f;
    [SerializeField] private bool _destroyGameObjectOnDeath;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<EnemyHealth, float> HealthChanged;
    public event Action<EnemyHealth, DamageInfo> Damaged;
    public event Action<EnemyHealth> Died;

    private void Awake()
    {
        CurrentHealth = _maxHealth;
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (IsDead || damageInfo.Amount <= 0f)
            return;

        float previousHealth = CurrentHealth;
        CurrentHealth = Mathf.Max(0f, CurrentHealth - damageInfo.Amount);
        
        float appliedDamage = previousHealth - CurrentHealth;

        if(appliedDamage <= 0f)
            return;

        DamageInfo appliedDamageInfo = damageInfo.WithAmount(appliedDamage);

        HealthChanged?.Invoke(this, CurrentHealth);
        Damaged?.Invoke(this, appliedDamageInfo);

        if (CurrentHealth <= 0f)
            Die();

    }

    public void RestoreFullHealth()
    {
        if (IsDead)
            return;

        CurrentHealth = _maxHealth;
        HealthChanged?.Invoke(this, CurrentHealth);
    }

    private void Die()
    {
        IsDead = true;
        Died?.Invoke(this);

        if (_destroyGameObjectOnDeath)
            Destroy(gameObject);
    }

    private void OnValidate()
    {
        _maxHealth = Mathf.Max(1f, _maxHealth);
    }
}
