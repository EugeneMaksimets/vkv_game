public class EnemyIdleState : IEnemyState
{
    private readonly EnemyMovement _movement;
    private readonly EnemyAnimation _animation;

    public EnemyIdleState(EnemyMovement movement, EnemyAnimation animation)
    {
        _movement = movement;
        _animation = animation;
    }

    public void Enter()
    {
        _movement.Stop();
        _animation.PlayMoveAnimation(false);
    }

    public void UpdateState()
    {
    }

    public void Exit()
    {
    }
}
