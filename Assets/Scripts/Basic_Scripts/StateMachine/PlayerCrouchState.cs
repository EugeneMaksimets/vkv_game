using UnityEngine;

public class PlayerCrouchState : IPlayerState
{
    private PlayerStateController _playerStateController;

    public PlayerCrouchState(PlayerStateController playerStateController)
    {
        _playerStateController = playerStateController;
    }

    public void Enter()
    {
        _playerStateController.Animator.SetBool("IsCrouching", true);
        
        // Уменьшаем коллайдер для приседания
        _playerStateController.Collider.size = new Vector2(_playerStateController.OriginalColliderSize.x, _playerStateController.OriginalColliderSize.y / 2);
        _playerStateController.Collider.offset = new Vector2(_playerStateController.OriginalColliderOffset.x, _playerStateController.OriginalColliderOffset.y - (_playerStateController.OriginalColliderSize.y / 4));
    }

    public void UpdateState()
    {
        // Передаем абсолютное значение горизонтального ввода в параметр Speed для анимации ходьбы на корточках
        _playerStateController.Animator.SetFloat("Speed", Mathf.Abs(_playerStateController.HorizontalInput));
    }

    public void Exit()
    {
        _playerStateController.Animator.SetBool("IsCrouching", false);

        // Возвращаем коллайдер в исходное состояние
        _playerStateController.Collider.size = _playerStateController.OriginalColliderSize;
        _playerStateController.Collider.offset = _playerStateController.OriginalColliderOffset;
    }
}