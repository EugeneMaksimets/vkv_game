using UnityEngine;

public class PlayerDieState : IPlayerState
{
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;

    public PlayerDieState(PlayerMovement movement, PlayerAnimation animation)
    {
        _playerMovement = movement;
        _playerAnimation = animation;
    }

    public void Enter()
    {
        _playerMovement.StopHorizontalMovement();

        _playerAnimation.PlayDie();
    }

    public void UpdateState()
    {
        // Возврат выполняется через HandleRevived в PlayerBrain.
    }

    public void Exit() { }
}