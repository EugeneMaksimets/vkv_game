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
        EnemyAnimation animation
        )
    {
        IdleState = new EnemyIdleState(movement, animation);
        WalkState = new EnemyWalkState(movement, target, animation);

        AttackState = new EnemyAttackState(
            enemy,
            movement,
            target,
            animation
            );

        DamagedState = new EnemyGetDamageState(movement, animation);
        DieState = new EnemyDieState(movement, animation);
    }
}