using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyTarget))]
public abstract class Enemy : MonoBehaviour
{
    protected EnemyHealth Health {get; private set;}
    protected EnemyTarget Target {get; private set;}

    protected virtual void Awake()
    {
        Health = GetComponent<EnemyHealth>();
        Target = GetComponent<EnemyTarget>();
    }

    public virtual void TryAttack()
    {
        // Реализация атаки в наследниках
    }

    public virtual bool IsTargetInRange()
    {
        return false; // Реализация проверки диапазона в наследниках
    }
    public virtual bool CanAttackNow()
    {
        return false; // Реализация проверки возможности атаки в наследниках
    } 
    public virtual void SetAutomaticAttack(bool automatic)
    {
        // Реализация установки автоматической атаки в наследниках
    }
}
