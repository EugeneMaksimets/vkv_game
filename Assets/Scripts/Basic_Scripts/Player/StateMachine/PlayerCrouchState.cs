
using UnityEngine;

public class PlayerCrouchState : IPlayerState
{
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;

    public PlayerCrouchState(PlayerMovement movement, PlayerAnimation animation)
    {
        _playerMovement = movement;
        _playerAnimation = animation;
    }

    public void Enter()
    {
        _playerAnimation.SetCrouching(true);
        _playerMovement.SetCrouching(true);
    }

    public void UpdateState()
    {
        _playerAnimation.SetSpeed(
            Mathf.Abs(_playerMovement.HorizontalInput)
        );
    }

    public void Exit()
    {
        _playerAnimation.SetCrouching(false);
        _playerMovement.SetCrouching(false);
    }
}
