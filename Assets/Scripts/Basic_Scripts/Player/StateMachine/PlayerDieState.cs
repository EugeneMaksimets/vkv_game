
using UnityEngine;

public class PlayerDieState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;

    public PlayerDieState(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }

    public void Enter()
    {
        _playerBrain.Rb.linearVelocity = new Vector2(
            0f,
            _playerBrain.Rb.linearVelocity.y
        );

        _playerBrain.Animation.PlayDie();
    }

    public void UpdateState()
    {
        // Возврат выполняется через HandleRevived в PlayerBrain.
    }

    public void Exit() { }
}
