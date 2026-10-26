/// <summary>
/// Контракт для объекта, которому можно нанести урон.
/// Его реализует компонент здоровья, например EnemyHealth.
/// </summary>
public interface IDamageable
{
    void TakeDamage(float damage);
}
