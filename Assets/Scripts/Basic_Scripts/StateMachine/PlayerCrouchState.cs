using UnityEngine;

public class PlayerCrouchState : IPlayerState
{
    private PlayerMovement _player;

    public PlayerCrouchState(PlayerMovement player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.Animator.SetBool("IsCrouching", true);
        
        // Уменьшаем коллайдер для приседания
        _player.Collider.size = new Vector2(_player.OriginalColliderSize.x, _player.OriginalColliderSize.y / 2);
        _player.Collider.offset = new Vector2(_player.OriginalColliderOffset.x, _player.OriginalColliderOffset.y - (_player.OriginalColliderSize.y / 4));
    }

    public void UpdateState()
    {
        // Передаем абсолютное значение горизонтального ввода в параметр Speed для анимации ходьбы на корточках
        _player.Animator.SetFloat("Speed", Mathf.Abs(_player.HorizontalInput));
    }

    public void Exit()
    {
        _player.Animator.SetBool("IsCrouching", false);

        // Возвращаем коллайдер в исходное состояние
        _player.Collider.size = _player.OriginalColliderSize;
        _player.Collider.offset = _player.OriginalColliderOffset;
    }
}