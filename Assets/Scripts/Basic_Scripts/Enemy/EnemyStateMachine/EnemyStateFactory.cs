public class EnemyStateFactory
{
    public EnemyIdleState IdleState { get; }
    public EnemyWalkState WalkState { get; }
    public EnemyAttackState AttackState { get; }
    public EnemyGetDamageState GetDamageState { get; }
    public EnemyDieState DieState { get; }

    public EnemyStateFactory(EnemyMovement movement, EnemyTarget target, EnemyAnimation animation)
    {
        IdleState = new EnemyIdleState(movement, animation);
        WalkState = new EnemyWalkState(movement, target, animation);
        AttackState = new EnemyAttackState(movement, target, animation);
        GetDamageState = new EnemyGetDamageState(movement, animation);
        DieState = new EnemyDieState(movement, animation);
    }
}
