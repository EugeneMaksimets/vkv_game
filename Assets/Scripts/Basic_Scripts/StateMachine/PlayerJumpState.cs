
using UnityEngine;

public class PlayerJumpState : IPlayerState
{
    private PlayerStateController _playerStateControler;
    private bool _hasLeftGround;

    public PlayerJumpState(PlayerStateController playerStateController)
    {
        _playerStateControler = playerStateController;
    }

    public void Enter()
    {
        _hasLeftGround = false;

        // Запускаем прыжок
        _playerStateControler.Rb.linearVelocity = new Vector2(
            _playerStateControler.Rb.linearVelocity.x,
            _playerStateControler.JumpForce
        );

        _playerStateControler.Animator.ResetTrigger("Jump");
        _playerStateControler.Animator.SetTrigger("Jump");
    }

    public void UpdateState()
    {
        // Сначала обязательно ждём отрыва от земли
        if (!_hasLeftGround)
        {
            if (!_playerStateControler.IsGrounded)
                _hasLeftGround = true;

            return;
        }

        // Возвращаемся в обычное состояние только после приземления
        if (_playerStateControler.IsGrounded &&
            _playerStateControler.Rb.linearVelocity.y <= 0.01f)
        {
            if (Mathf.Abs(_playerStateControler.HorizontalInput) > 0.1f)
            {
                _playerStateControler.StateMachine.ChangeState(_playerStateControler.RunState);
            }
            else
            {
                _playerStateControler.StateMachine.ChangeState(_playerStateControler.IdleState);
            }
        }
    }

    public void Exit()
    {
    }
}
