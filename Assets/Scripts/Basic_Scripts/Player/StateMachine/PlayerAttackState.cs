public class PlayerAttackState : IPlayerState
{
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;

    public PlayerAttackState(PlayerMovement movement, PlayerAnimation animation)
    {
        _playerMovement = movement;
        _playerAnimation = animation;
    }

    public void Enter()
    {
        _playerMovement.StopHorizontalMovement();
        _playerAnimation.PlayAttack();
    }

    public void UpdateState()
    {
    }

    public void Exit()
    {
    }
}