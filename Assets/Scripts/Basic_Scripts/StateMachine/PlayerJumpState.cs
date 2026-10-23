using UnityEngine;

public class PlayerJumpState : IPlayerState
{
    private PlayerMovement _player;

    public PlayerJumpState(PlayerMovement player)
    {
        _player = player;
    }

    public void Enter()
    {
        // Задаем вертикальную скорость для прыжка и триггер в аниматоре
        _player.Rb.linearVelocity = new Vector2(_player.Rb.linearVelocity.x, _player.JumpForce);
        _player.Animator.SetTrigger("Jump");
    }

    public void UpdateState()
    {
        // Если приземлились (IsGrounded == true) и летим вниз или остановились по вертикали
        if (_player.IsGrounded && _player.Rb.linearVelocity.y <= 0.01f)
        {
            // Если есть горизонтальный ввод — переходим в бег, иначе в покой
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

    public void Exit() { }
}