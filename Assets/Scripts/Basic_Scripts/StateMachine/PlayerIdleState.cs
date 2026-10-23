// using UnityEngine;

// public class PlayerIdleState : IPlayerState
// {
//     private PlayerMovement _player;

//     public PlayerIdleState(PlayerMovement player)
//     {
//         _player = player;
//     }

//     public void Enter()
//     {
//         _player.Animator.SetFloat("Speed", 0f);
//     }

//     public void UpdateState()
//     {
//         // Если пошел горизонтальный ввод — переходим в бег
//         if (Mathf.Abs(_player.HorizontalInput) > 0.1f)
//         {
//             _player.StateMachine.ChangeState(_player.RunState);
//         }
//     }

//     public void Exit() { }
// }