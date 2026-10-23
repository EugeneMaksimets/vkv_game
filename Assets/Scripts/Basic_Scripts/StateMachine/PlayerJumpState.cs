
using UnityEngine;

public class PlayerJumpState : IPlayerState
{
    private PlayerStateController _player;
    private bool _hasLeftGround;

    public PlayerJumpState(PlayerStateController player)
    {
        _player = player;
    }

    public void Enter()
    {
        _hasLeftGround = false;

        // Запускаем прыжок
        _player.Rb.linearVelocity = new Vector2(
            _player.Rb.linearVelocity.x,
            _player.JumpForce
        );

        _player.Animator.ResetTrigger("Jump");
        _player.Animator.SetTrigger("Jump");
    }

    public void UpdateState()
    {
        // Сначала обязательно ждём отрыва от земли
        if (!_hasLeftGround)
        {
            if (!_player.IsGrounded)
                _hasLeftGround = true;

            return;
        }

        // Возвращаемся в обычное состояние только после приземления
        if (_player.IsGrounded &&
            _player.Rb.linearVelocity.y <= 0.01f)
        {
            if (Mathf.Abs(_player.HorizontalInput) > 0.1f)
            {
                _player.StateMachine.ChangeState(_player.RunState);
            }
            else
            {
                _player.StateMachine.ChangeState(_player.IdleState);
            }
        }
    }

    public void Exit()
    {
    }
}
