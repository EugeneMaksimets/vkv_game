// using UnityEngine;

// public class PlayerCrouchState : IPlayerState
// {
//     private PlayerMovement _player;

//     public PlayerCrouchState(PlayerMovement player)
//     {
//         _player = player;
//     }

//     public void Enter()
//     {
//         _player.Animator.SetBool("IsCrouching", true);
//         // Уменьшаем коллайдер
//         _player.Collider.size = new Vector2(_player.OriginalColliderSize.x, _player.OriginalColliderSize.y * 0.5f);
//         _player.Collider.offset = new Vector2(_player.OriginalColliderOffset.x, _player.OriginalColliderOffset.y - (_player.OriginalColliderSize.y * 0.25f));
//     }

//     public void UpdateState()
//     {
//         // Логика выхода из приседания обрабатывается в методе StandUp / вводе
//     }

//     public void Exit()
//     {
//         _player.Animator.SetBool("IsCrouching", false);
//         // Возвращаем коллайдер
//         _player.Collider.size = _player.OriginalColliderSize;
//         _player.Collider.offset = _player.OriginalColliderOffset;
//     }
// }