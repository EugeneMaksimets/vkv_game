public class EnemyIdleState : IEnemyState
{
    private readonly EnemyMovement _movement;
    private readonly EnemyAnimation _animation;
    private readonly EnemyTarget _target;

    public EnemyIdleState(EnemyMovement movement, EnemyAnimation animation, EnemyTarget target)
    {
        _movement = movement;
        _animation = animation;
        _target = target;
    }

    public void Enter()
    {
        _movement.Stop();
        _animation.PlayMoveAnimation(false);
    }

    public void UpdateState()
    {
        _movement.FaceTarget(_target.TargetTransform);
    }

    public void Exit()
    {
    }
}
