using UnityEngine;

public class PlayerRunState : IPlayerState
{
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;

    public PlayerRunState(PlayerMovement movement, PlayerAnimation animation)
    {
        _playerMovement = movement;
        _playerAnimation = animation;
    }

    public void Enter() { }

    public void UpdateState()
    {
        _playerAnimation.SetSpeed(
            Mathf.Abs(_playerMovement.HorizontalInput)
        );
    }

    public void Exit()
    {
        _playerAnimation.SetSpeed(0f);
    }
}