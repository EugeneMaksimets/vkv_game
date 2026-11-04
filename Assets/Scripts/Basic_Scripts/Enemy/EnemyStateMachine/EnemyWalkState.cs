public class EnemyWalkState : IEnemyState
{
    private readonly EnemyMovement _movement;
    private readonly EnemyTarget _target;
    private readonly EnemyAnimation _animation;

    public EnemyWalkState(EnemyMovement movement, EnemyTarget target, EnemyAnimation animation)
    {
        _movement = movement;
        _target = target;
        _animation = animation;
    }

    public void Enter()
    {
        _animation.PlayMoveAnimation(true);
    }

    public void UpdateState()
    {
        _movement.MoveTowards(_target.TargetTransform);
    }

    public void Exit()
    {
        _movement.Stop();
        _animation.PlayMoveAnimation(false);
    }
}
