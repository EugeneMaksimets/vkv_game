using UnityEngine;

public class PlayerRunState : IPlayerState
{
    private PlayerStateController _playerStateControler;

    public PlayerRunState(PlayerStateController playerStateController)
    {
        _playerStateControler = playerStateController;
    }

    public void Enter() { }

    public void UpdateState()
    {
        // Передаем текущую скорость в аниматор (абсолютное значение ввода)
        _playerStateControler.Animator.SetFloat("Speed", Mathf.Abs(_playerStateControler.HorizontalInput));

        // Если ввод прекратился (вернулся к 0) — возвращаемся в Idle
        if (Mathf.Abs(_playerStateControler.HorizontalInput) < 0.1f)
        {
            _playerStateControler.StateMachine.ChangeState(_playerStateControler.IdleState);
        }
    }

    public void Exit()
    {
        // При выходе из бега обнуляем скорость в аниматоре
        _playerStateControler.Animator.SetFloat("Speed", 0f);
    }
}