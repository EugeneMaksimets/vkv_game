/// <summary>
/// Останавливает врага и запускает анимацию смерти.
/// </summary>
public class EnemyDieState : IEnemyState
{
    private readonly EnemyStateController _controller;

    public EnemyDieState(EnemyStateController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Movement.Stop();
        _controller.SetMoveAnimation(false);
        _controller.PlayDieAnimation();
    }

    public void UpdateState()
    {
    }

    public void Exit()
    {
    }
}
