
using UnityEngine;

public class PlayerJumpState : IPlayerState
{
    private PlayerStateController _playerStateController;
    private bool _hasLeftGround;

    public PlayerJumpState(PlayerStateController playerStateController)
    {
        _playerStateController = playerStateController;
    }

    public void Enter()
    {
        _hasLeftGround = false;
        _playerStateController.Animator.ResetTrigger("Jump");
        _playerStateController.Animator.SetTrigger("Jump");
    }

    public void UpdateState()
    {
        // Сначала обязательно ждём отрыва от земли
        if (!_hasLeftGround)
        {
            if (!_playerStateController.IsGrounded)
                _hasLeftGround = true;

            return;
        }

        // Возвращаемся в обычное состояние только после приземления
        if (_playerStateController.IsGrounded &&
            _playerStateController.Rb.linearVelocity.y <= 0.01f)
        {
            if (Mathf.Abs(_playerStateController.HorizontalInput) > 0.1f)
            {
                _playerStateController.StateMachine.ChangeState(_playerStateController.RunState);
            }
            else
            {
                _playerStateController.StateMachine.ChangeState(_playerStateController.IdleState);
            }
        }
    }

    public void Exit()
    {
    }
}
