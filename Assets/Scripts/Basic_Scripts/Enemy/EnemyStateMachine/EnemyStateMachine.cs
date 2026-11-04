/// <summary>
/// Manages the current state of the enemy and handles transitions between states.
/// </summary>
public class EnemyStateMachine
{
    public IEnemyState CurrentState { get; private set; }

    public void Initialize(IEnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(IEnemyState newState)
    {
        if (newState == null || CurrentState == newState)
            return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.UpdateState();
    }

    public void RestartCurrentState()
    {
        if (CurrentState == null)
            return;

        CurrentState.Exit();
        CurrentState.Enter();
    }
}
