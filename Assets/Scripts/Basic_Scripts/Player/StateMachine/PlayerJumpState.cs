
using UnityEngine;

public class PlayerJumpState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;
    private bool _hasLeftGround;

    public PlayerJumpState(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }

    public void Enter()
    {
        _hasLeftGround = false;

        _playerBrain.Animation.PlayJump();
    }

    public void UpdateState()
    {
        if (!_hasLeftGround)
        {
            if (!_playerBrain.IsGrounded)
            {
                _hasLeftGround = true;
            }

            return;
        }

        if (_playerBrain.IsGrounded &&
            _playerBrain.Rb.linearVelocity.y <= 0.01f)
        {
            if (Mathf.Abs(_playerBrain.HorizontalInput) > 0.1f)
            {
                _playerBrain.StateMachine.ChangeState(
                    _playerBrain.RunState
                );
            }
            else
            {
                _playerBrain.StateMachine.ChangeState(
                    _playerBrain.IdleState
                );
            }
        }
    }

    public void Exit() { }
}
