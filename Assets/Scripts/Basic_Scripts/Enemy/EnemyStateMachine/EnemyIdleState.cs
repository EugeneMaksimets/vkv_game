/// <summary>
/// Враг ждёт, пока не появится цель вне радиуса атаки или возможность ударить.
/// </summary>
public class EnemyIdleState : IEnemyState
{
    private readonly EnemyStateController _controller;

    public EnemyIdleState(EnemyStateController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Movement.Stop();
        _controller.SetMoveAnimation(false);
    }

    public void UpdateState()
    {
        if (!_controller.CanMove)
        {
           _controller.Movement.FaceTarget(_controller.Target.Target); 
        }
        _controller.ChooseIdleWalkOrAttackState();
    }

    public void Exit()
    {
    }
}
