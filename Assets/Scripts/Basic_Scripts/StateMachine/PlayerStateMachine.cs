// public class PlayerStateMachine
// {
//     public IPlayerState CurrentState { get; private set; }

//     public void Initialize(IPlayerState startingState)
//     {
//         CurrentState = startingState;
//         CurrentState.Enter();
//     }

//     public void ChangeState(IPlayerState newState)
//     {
//         CurrentState?.Exit();
//         CurrentState = newState;
//         CurrentState.Enter();
//     }

//     public void Update()
//     {
//         CurrentState?.UpdateState();
//     }
// }