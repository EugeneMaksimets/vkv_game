using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;

    public PlayerIdleState(PlayerMovement movement, PlayerAnimation animation)
    {
        _playerMovement = movement;
        _playerAnimation = animation;
    }

    public void Enter()
    {
        _playerAnimation.SetSpeed(0f);
    }

    public void UpdateState()
    {
    }

    public void Exit()
    {
    }
}