public class EnemyDieState : IEnemyState
{
    private readonly EnemyMovement _movement;
    private readonly EnemyAnimation _animation;

    public EnemyDieState(EnemyMovement movement, EnemyAnimation animation)
    {
        _movement = movement;
        _animation = animation;
    }

    public void Enter()
    {
        _movement.Stop();
        _animation.PlayMoveAnimation(false);
        _animation.PlayDieAnimation();
    }

    public void UpdateState()
    {
    }

    public void Exit()
    {
    }
}
