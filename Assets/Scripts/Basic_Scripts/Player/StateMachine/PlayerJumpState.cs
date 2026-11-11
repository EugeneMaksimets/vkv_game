
using UnityEngine;

public class PlayerJumpState : IPlayerState
{
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;

    public bool HasLeftGround { get; private set; }

    public PlayerJumpState(PlayerMovement movement, PlayerAnimation animation)
    {
        _playerMovement = movement;
        _playerAnimation = animation;
    }

    public void Enter()
    {
        HasLeftGround = false;

        _playerAnimation.PlayJump();
    }

    public void UpdateState()
    {
        if (!HasLeftGround && !_playerMovement.IsGrounded)
        {
            HasLeftGround = true;
        }
    }

    public void Exit() { }
}
