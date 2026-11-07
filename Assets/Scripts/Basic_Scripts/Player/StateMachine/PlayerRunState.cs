using UnityEngine;

public class PlayerRunState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;

    public PlayerRunState(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }

    public void Enter() { }

    public void UpdateState()
    {
        _playerBrain.Animation.SetSpeed(
            Mathf.Abs(_playerBrain.HorizontalInput)
        );
    }

    public void Exit()
    {
        _playerBrain.Animation.SetSpeed(0f);
    }
}