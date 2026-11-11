public class PlayerStateFactory
{
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;
    private readonly PlayerInteraction _playerInteraction;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerInteractState InteractState { get; private set; }
    public PlayerGetDamageState GetDamageState { get; private set; }
    public PlayerDieState DieState { get; private set; }

    public PlayerStateFactory(
        PlayerMovement playerMovement,
        PlayerAnimation playerAnimation,
        PlayerInteraction playerInteraction
        )
    {
        _playerMovement = playerMovement;
        _playerAnimation = playerAnimation;
        _playerInteraction = playerInteraction;
    }

    public void CreateStates()
    {
        IdleState = new PlayerIdleState(_playerMovement, _playerAnimation);
        RunState = new PlayerRunState(_playerMovement, _playerAnimation);

        JumpState = new PlayerJumpState(
            _playerMovement,
            _playerAnimation);

        CrouchState = new PlayerCrouchState(_playerMovement, _playerAnimation);
        AttackState = new PlayerAttackState(_playerMovement, _playerAnimation);
        InteractState = new PlayerInteractState(_playerInteraction, _playerAnimation);
        GetDamageState = new PlayerGetDamageState(_playerMovement, _playerAnimation);
        DieState = new PlayerDieState(_playerMovement, _playerAnimation);
    }
}