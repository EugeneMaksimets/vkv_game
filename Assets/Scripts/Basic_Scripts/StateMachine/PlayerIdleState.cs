using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private PlayerStateController _playerStateController;

    public PlayerIdleState(PlayerStateController playerStateController)
    {
        _playerStateController = playerStateController;
    }

    public void Enter()
    {
        // В состоянии покоя скорость в аниматоре сбрасывается в 0
        _playerStateController.Animator.SetFloat("Speed", 0f);
    }

    public void UpdateState()
    {
        // Если игрок дал горизонтальный ввод (с клавиатуры или джойстика) — переходим в бег
        if (Mathf.Abs(_playerStateController.HorizontalInput) > 0.1f)
        {
            _playerStateController.StateMachine.ChangeState(_playerStateController.RunState);
        }
    }

    public void Exit() { }
}