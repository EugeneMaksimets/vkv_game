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
        // Передаем текущую скорость в аниматор (абсолютное значение ввода)
        _playerBrain.Animator.SetFloat("Speed", Mathf.Abs(_playerBrain.HorizontalInput));

        // Если ввод прекратился (вернулся к 0) — возвращаемся в Idle
        if (Mathf.Abs(_playerBrain.HorizontalInput) < 0.1f)
        {
            _playerBrain.StateMachine.ChangeState(_playerBrain.IdleState);
        }
    }

    public void Exit()
    {
        // При выходе из бега обнуляем скорость в аниматоре
        _playerBrain.Animator.SetFloat("Speed", 0f);
    }
}