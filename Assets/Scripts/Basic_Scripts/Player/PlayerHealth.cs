using System;
using UnityEngine;

/// <summary>
/// Хранит здоровье игрока и реализует IDamageable,
/// поэтому враги могут наносить игроку урон через общий контракт.
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float _maxHealth = 5f;

    [Tooltip("Здоровье, с которым игрок вернётся в игру при вызове Revive().")]
    [SerializeField] private float _reviveHealth = 1f;

    public float MaxHealth => _maxHealth;

    public float CurrentHealth { get; private set; }

    public bool IsDead { get; private set; }

    /// <summary>Вызывается после каждого изменения CurrentHealth.</summary>
    public event Action<PlayerHealth, float> HealthChanged;

    /// <summary>Вызывается после успешного получения урона. Второй параметр — реальный нанесённый урон.</summary>
    public event Action<PlayerHealth, DamageInfo> Damaged;

    /// <summary>Вызывается после успешного лечения. Второй параметр — реальное восстановленное здоровье.</summary>
    public event Action<PlayerHealth, float> Healed;

    /// <summary>Вызывается один раз, когда здоровье достигает нуля.</summary>
    public event Action<PlayerHealth> Died;

    /// <summary>Вызывается после успешного воскрешения.</summary>
    public event Action<PlayerHealth> Revived;

    private void Awake()
    {
        // Новый игрок начинает с полным здоровьем.
        CurrentHealth = _maxHealth;
    }

    /// <summary>
    /// Реализация IDamageable. Уменьшает здоровье и вызывает события урона и смерти.
    /// </summary>
    public void TakeDamage(DamageInfo damageInfo)
    {
        Debug.Log("Player: " + "Get DAMAGE, currentHealth:" + CurrentHealth);
        // Мёртвый игрок больше не получает урон до воскрешения.
        if (IsDead || damageInfo.Amount <= 0f)
            return;

        float previousHealth = CurrentHealth;
        CurrentHealth = Mathf.Max(0f, CurrentHealth - damageInfo.Amount);
        float appliedDamage = previousHealth - CurrentHealth;

        // Если здоровье уже было нулевым, события повторно не вызываются.
        if (appliedDamage <= 0f)
            return;

        // real damage for futer exemple if have Armor
        DamageInfo appliedDamageInfo = damageInfo.WithAmount(appliedDamage);

        HealthChanged?.Invoke(this, CurrentHealth);
        Damaged?.Invoke(this, appliedDamageInfo);

        if (CurrentHealth <= 0f)
            Die();
    }

    /// <summary>
    /// Восстанавливает здоровье до MaxHealth. Мёртвого игрока лечить нельзя — сначала Revive.
    /// </summary>
    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f)
            return;

        float previousHealth = CurrentHealth;
        CurrentHealth = Mathf.Min(_maxHealth, CurrentHealth + amount);
        float restoredHealth = CurrentHealth - previousHealth;

        if (restoredHealth <= 0f)
            return;

        HealthChanged?.Invoke(this, CurrentHealth);
        Healed?.Invoke(this, restoredHealth);
    }

    /// <summary>
    /// Воскрешает игрока со значением _reviveHealth из Inspector.
    /// </summary>
    public void Revive()
    {
        Revive(_reviveHealth);
    }

    /// <summary>
    /// Воскрешает игрока с указанным количеством здоровья.
    /// Значение ограничивается диапазоном от 1 до MaxHealth.
    /// </summary>
    public void Revive(float health)
    {
        if (!IsDead)
            return;

        IsDead = false;
        CurrentHealth = Mathf.Clamp(health, 1f, _maxHealth);

        HealthChanged?.Invoke(this, CurrentHealth);
        Revived?.Invoke(this);
    }

    private void Die()
    {
        IsDead = true;
        Died?.Invoke(this);
    }

    private void OnValidate()
    {
        // Защищаем Inspector от недопустимых значений.
        _maxHealth = Mathf.Max(1f, _maxHealth);
        _reviveHealth = Mathf.Clamp(_reviveHealth, 1f, _maxHealth);
    }
}
