using UnityEngine;

/// <summary>
/// Данные одного попадания.
/// Передаются от атакующего объекта в компонент здоровья цели.
/// </summary>
public class DamageInfo
{
    // Damage Amount
    public float Amount {get;}
    // Position hit or bullet
    public Vector2 SourcePosition{get;}
    public float KnockbackSpeed {get;}
    // in sec 
    public float KnockbackDuration {get;}

    public DamageInfo(float amount, 
    Vector2 sourcePosition, 
    float knockbackSpeed = 0f, 
    float knockbackDuration = 0f)
    {
        Amount = Mathf.Max(0f, amount);
        SourcePosition = sourcePosition;
        KnockbackSpeed = Mathf.Max(0f, knockbackSpeed);
        KnockbackDuration = Mathf.Max(0f, knockbackDuration);
    }

    /// <summary>
    /// Создаёт копию попадания с фактически нанесённым уроном.
    /// Это пригодится позднее для брони, щитов и сопротивлений.
    /// </summary>
    public DamageInfo WithAmount (float amount)
    {
        return new DamageInfo(
            amount,
            SourcePosition,
            KnockbackSpeed,
            KnockbackDuration
        );
    }
}
