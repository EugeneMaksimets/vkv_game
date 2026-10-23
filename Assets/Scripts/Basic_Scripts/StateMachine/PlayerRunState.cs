using UnityEngine;

public class PlayerRunState : IPlayerState
{
    private PlayerStateController _player;

    public PlayerRunState(PlayerStateController player)
    {
        _player = player;
    }

    public void Enter() { }

    public void UpdateState()
    {
        // Передаем текущую скорость в аниматор (абсолютное значение ввода)
        _player.Animator.SetFloat("Speed", Mathf.Abs(_player.HorizontalInput));

        // Если ввод прекратился (вернулся к 0) — возвращаемся в Idle
        if (Mathf.Abs(_player.HorizontalInput) < 0.1f)
        {
            _player.StateMachine.ChangeState(_player.IdleState);
        }
    }

    public void Exit()
    {
        // При выходе из бега обнуляем скорость в аниматоре
        _player.Animator.SetFloat("Speed", 0f);
    }
}