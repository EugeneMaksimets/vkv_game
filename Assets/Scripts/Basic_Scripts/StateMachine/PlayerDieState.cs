/// <summary>
/// Состояние игрока после PlayerHealth.Died.
/// Оно удерживает игрока неподвижным до события PlayerHealth.Revived.
/// </summary>
public class PlayerDieState : IPlayerState
{
    private readonly PlayerStateController _playerStateController;

    public PlayerDieState(PlayerStateController playerStateController)
    {
        _playerStateController = playerStateController;
    }

    public void Enter()
    {
        _playerStateController.StopMoving();
        _playerStateController.PlayDieAnimation();
    }

    public void UpdateState()
    {
        // Переход обратно выполняет HandleRevived в PlayerStateController.
    }

    public void Exit()
    {
    }
}
