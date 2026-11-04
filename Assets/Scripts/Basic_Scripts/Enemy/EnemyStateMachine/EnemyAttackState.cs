public class EnemyAttackState : IEnemyState
{
    private readonly EnemyMovement _movement;
    private readonly EnemyTarget _target;
    private readonly EnemyAnimation _animation;

    public EnemyAttackState(EnemyMovement movement, EnemyTarget target, EnemyAnimation animation)
    {
        _movement = movement;
        _target = target;
        _animation = animation;
    }

    public void Enter()
    {
        _movement.Stop();
        _movement.FaceTarget(_target.TargetTransform);

        _animation.PlayMoveAnimation(false);
        _animation.PlayAttackAnimation();
    }

    public void UpdateState()
    {
        
    }

    public void Exit()
    {
    }
}
