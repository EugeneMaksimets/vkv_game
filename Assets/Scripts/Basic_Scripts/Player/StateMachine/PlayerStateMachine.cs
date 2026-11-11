public class PlayerStateMachine {
    public IPlayerState CurrentState {get; private set;}

    public void Initialize(IPlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(IPlayerState newState)
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
}