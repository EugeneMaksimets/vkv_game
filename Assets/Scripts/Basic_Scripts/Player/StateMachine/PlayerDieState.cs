/// <summary>
/// Состояние игрока после PlayerHealth.Died.
/// Игрок остаётся в этом состоянии до события PlayerHealth.Revived.
/// </summary>
public class PlayerDieState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;

    public PlayerDieState(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }

    public void Enter()
    {
        // Останавливаем горизонтальное движение,
        // сохраняя вертикальную скорость Rigidbody2D.
        _playerBrain.Rb.linearVelocity =
            new UnityEngine.Vector2(
                0f,
                _playerBrain.Rb.linearVelocity.y
            );

        _playerBrain.Animator.SetTrigger("Die");
    }

    public void UpdateState()
    {
        // Переход обратно выполняет HandleRevived в PlayerBrain.
    }

    public void Exit()
    {
    }
}