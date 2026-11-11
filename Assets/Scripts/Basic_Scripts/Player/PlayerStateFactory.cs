public class PlayerStateFactory
{
    private readonly PlayerBrain _playerBrain;
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;
    private readonly PlayerInteraction _playerInteraction;
    private readonly PlayerHealth _playerHealth;
    private readonly float _attackDuration;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerInteractState InteractState { get; private set; }
    public PlayerGetDamageState GetDamageState { get; private set; }
    public PlayerDieState DieState { get; private set; }

    public PlayerStateFactory(
        PlayerBrain playerBrain,
        PlayerMovement playerMovement,
        PlayerAnimation playerAnimation,
        PlayerInteraction playerInteraction,
        PlayerHealth playerHealth,
        float attackDuration)
    {
        _playerBrain = playerBrain;
        _playerMovement = playerMovement;
        _playerAnimation = playerAnimation;
        _playerInteraction = playerInteraction;
        _playerHealth = playerHealth;
        _attackDuration = attackDuration;
    }

    public void CreateStates()
    {
        IdleState = new PlayerIdleState(_playerMovement, _playerAnimation);
        RunState = new PlayerRunState(_playerMovement, _playerAnimation);

        JumpState = new PlayerJumpState(
            _playerMovement,
            _playerAnimation);

        CrouchState = new PlayerCrouchState(_playerMovement, _playerAnimation);
        AttackState = new PlayerAttackState(_playerBrain, _playerMovement, _playerAnimation, _attackDuration);
        InteractState = new PlayerInteractState(_playerBrain, _playerInteraction, _playerAnimation);
        GetDamageState = new PlayerGetDamageState(_playerBrain, _playerMovement, _playerAnimation, _playerHealth);
        DieState = new PlayerDieState(_playerMovement, _playerAnimation);
    }
}