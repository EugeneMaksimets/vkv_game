/// <summary>
/// Враг идёт к цели, пока та не окажется в радиусе атаки.
/// </summary>
public class EnemyWalkState : IEnemyState
{
    private readonly EnemyStateController _controller;

    public EnemyWalkState(EnemyStateController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        _controller.SetMoveAnimation(true);
    }

    public void UpdateState()
    {
        if (!_controller.HasTarget || _controller.IsTargetInAttackRange)
        {
            _controller.ChooseIdleWalkOrAttackState();
            return;
        }

        _controller.Movement.MoveTowards(_controller.Target.Target);
    }

    public void Exit()
    {
        _controller.Movement.Stop();
        _controller.SetMoveAnimation(false);
    }
}
