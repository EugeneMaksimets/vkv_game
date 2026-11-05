
using UnityEngine;

public class PlayerCrouchState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;

    public PlayerCrouchState(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }

    public void Enter()
    {
        _playerBrain.Animation.SetCrouching(true);
    }

    public void UpdateState()
    {
        _playerBrain.Animation.SetSpeed(
            Mathf.Abs(_playerBrain.HorizontalInput)
        );
    }

    public void Exit()
    {
        _playerBrain.Animation.SetCrouching(false);
    }
}
