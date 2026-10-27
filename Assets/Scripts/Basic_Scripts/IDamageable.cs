/// <summary>
/// Общий контракт объектов, которые могут получать урон.
/// </summary>
public interface IDamageable
{
    public void TakeDamage(DamageInfo damageInfo);
}
