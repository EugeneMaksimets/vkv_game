using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;
    public PlayerIdleState(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }

    public void Enter()
    {
        // In IDLE speed = 0
        _playerBrain.Animation.SetSpeed(0f);
    }

    public void UpdateState()
    {
        // Input = run
        if (Mathf.Abs(_playerBrain.HorizontalInput) > 0.1f)
        {
            _playerBrain.StateMachine.ChangeState(_playerBrain.RunState);
        }
    }

    public void Exit() { }
}