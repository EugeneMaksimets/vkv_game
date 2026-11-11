public class EnemyStateFactory
{
    public EnemyIdleState IdleState { get; }
    public EnemyWalkState WalkState { get; }
    public EnemyAttackState AttackState { get; }
    public EnemyGetDamageState DamagedState { get; }
    public EnemyDieState DieState { get; }

    public EnemyStateFactory(
        Enemy enemy,
        EnemyMovement movement,
        EnemyTarget target,
        EnemyAnimation animation,
        float attackFallbackTimeout,
        float getDamageFallbackTimeout
        )
    {
        IdleState = new EnemyIdleState(movement, animation, target);
        WalkState = new EnemyWalkState(movement, target, animation);

        AttackState = new EnemyAttackState(
            enemy,
            movement,
            target,
            animation,
            attackFallbackTimeout
            );

        DamagedState = new EnemyGetDamageState(movement, animation, getDamageFallbackTimeout);
        DieState = new EnemyDieState(movement, animation);
    }
}