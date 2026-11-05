public class PlayerStateFactory
{
    private readonly PlayerBrain _playerBrain;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerInteractState InteractState { get; private set; }
    public PlayerGetDamageState GetDamageState { get; private set; }
    public PlayerDieState DieState { get; private set; }

    public PlayerStateFactory (PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }
    public void CreateStates()
    {
        IdleState = new PlayerIdleState(_playerBrain);
        RunState = new PlayerRunState(_playerBrain);
        JumpState = new PlayerJumpState(_playerBrain);
        CrouchState = new PlayerCrouchState(_playerBrain);
        AttackState = new PlayerAttackState(_playerBrain);
        InteractState = new PlayerInteractState(_playerBrain);
        GetDamageState = new PlayerGetDamageState(_playerBrain);
        DieState = new PlayerDieState(_playerBrain);
    }
}
