using UnityEngine;

public class PlayerRunState : IPlayerState
{
    private PlayerStateController _playerStateController;

    public PlayerRunState(PlayerStateController playerStateController)
    {
        _playerStateController = playerStateController;
    }

    public void Enter() { }

    public void UpdateState()
    {
        // Передаем текущую скорость в аниматор (абсолютное значение ввода)
        _playerStateController.Animator.SetFloat("Speed", Mathf.Abs(_playerStateController.HorizontalInput));

        // Если ввод прекратился (вернулся к 0) — возвращаемся в Idle
        if (Mathf.Abs(_playerStateController.HorizontalInput) < 0.1f)
        {
            _playerStateController.StateMachine.ChangeState(_playerStateController.IdleState);
        }
    }

    public void Exit()
    {
        // При выходе из бега обнуляем скорость в аниматоре
        _playerStateController.Animator.SetFloat("Speed", 0f);
    }
}