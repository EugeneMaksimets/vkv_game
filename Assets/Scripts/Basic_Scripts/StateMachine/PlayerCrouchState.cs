using UnityEngine;

public class PlayerCrouchState : IPlayerState
{
    private PlayerStateController _playerStateControler;

    public PlayerCrouchState(PlayerStateController playerStateController)
    {
        _playerStateControler = playerStateController;
    }

    public void Enter()
    {
        _playerStateControler.Animator.SetBool("IsCrouching", true);
        
        // Уменьшаем коллайдер для приседания
        _playerStateControler.Collider.size = new Vector2(_playerStateControler.OriginalColliderSize.x, _playerStateControler.OriginalColliderSize.y / 2);
        _playerStateControler.Collider.offset = new Vector2(_playerStateControler.OriginalColliderOffset.x, _playerStateControler.OriginalColliderOffset.y - (_playerStateControler.OriginalColliderSize.y / 4));
    }

    public void UpdateState()
    {
        // Передаем абсолютное значение горизонтального ввода в параметр Speed для анимации ходьбы на корточках
        _playerStateControler.Animator.SetFloat("Speed", Mathf.Abs(_playerStateControler.HorizontalInput));
    }

    public void Exit()
    {
        _playerStateControler.Animator.SetBool("IsCrouching", false);

        // Возвращаем коллайдер в исходное состояние
        _playerStateControler.Collider.size = _playerStateControler.OriginalColliderSize;
        _playerStateControler.Collider.offset = _playerStateControler.OriginalColliderOffset;
    }
}