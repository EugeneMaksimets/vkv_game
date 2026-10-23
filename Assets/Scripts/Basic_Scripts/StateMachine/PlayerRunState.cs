// using UnityEngine;

// public class PlayerRunState : IPlayerState
// {
//     private PlayerMovement _player;

//     public PlayerRunState(PlayerMovement player)
//     {
//         _player = player;
//     }

//     public void Enter() { }

//     public void UpdateState()
//     {
//         _player.Animator.SetFloat("Speed", Mathf.Abs(_player.HorizontalInput));

//         // Если остановились — возвращаемся в Idle
//         if (Mathf.Abs(_player.HorizontalInput) < 0.1f)
//         {
//             _player.StateMachine.ChangeState(_player.IdleState);
//         }
//     }

//     public void Exit()
//     {
//         _player.Animator.SetFloat("Speed", 0f);
//     }
// }