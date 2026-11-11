
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyTarget))]
public abstract class Enemy : MonoBehaviour
{
    protected EnemyHealth Health { get; private set; }
    protected EnemyTarget Target { get; private set; }

    protected virtual void Awake()
    {
        Health = GetComponent<EnemyHealth>();
        Target = GetComponent<EnemyTarget>();
    }

    // Try to attack the target if possible. This method should be called when the enemy is in the attack state and the attack animation reaches the point where damage should be applied.
    public abstract void TryAttack();

    // Check if the target is within the attack range. This method should be called to determine if the enemy can initiate an attack.
    public abstract bool IsTargetInRange();

    // Check if the enemy can attack now. This method should be called to determine if the enemy is ready to perform an attack, considering cooldowns or other conditions.
    public abstract bool CanAttackNow();

    public abstract void StartAttackCooldown();
}
