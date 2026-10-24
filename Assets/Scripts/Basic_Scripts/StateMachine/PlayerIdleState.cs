using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private PlayerStateController _playerStateControler;

    public PlayerIdleState(PlayerStateController playerStateController)
    {
        _playerStateControler = playerStateController;
    }

    public void Enter()
    {
        // В состоянии покоя скорость в аниматоре сбрасывается в 0
        _playerStateControler.Animator.SetFloat("Speed", 0f);
    }

    public void UpdateState()
    {
        // Если игрок дал горизонтальный ввод (с клавиатуры или джойстика) — переходим в бег
        if (Mathf.Abs(_playerStateControler.HorizontalInput) > 0.1f)
        {
            _playerStateControler.StateMachine.ChangeState(_playerStateControler.RunState);
        }
    }

    public void Exit() { }
}