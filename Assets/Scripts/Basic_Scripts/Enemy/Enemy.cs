
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

    public abstract void TryAttack();

    public abstract bool IsTargetInRange();

    public abstract bool CanAttackNow();
}
