
using UnityEngine;

public class PlayerCrouchState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;

    public PlayerCrouchState(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }

    public void Enter()
    {
        _playerBrain.Animator.SetBool("IsCrouching", true);

        // Уменьшаем коллайдер для приседания.
        _playerBrain.Collider.size = new Vector2(
            _playerBrain.OriginalColliderSize.x,
            _playerBrain.OriginalColliderSize.y / 2);

        _playerBrain.Collider.offset = new Vector2(
            _playerBrain.OriginalColliderOffset.x,
            _playerBrain.OriginalColliderOffset.y -
            (_playerBrain.OriginalColliderSize.y / 4));
    }

    public void UpdateState()
    {
        // Передаём горизонтальный ввод в Animator.
        _playerBrain.Animator.SetFloat(
            "Speed",
            Mathf.Abs(_playerBrain.HorizontalInput));
    }

    public void Exit()
    {
        _playerBrain.Animator.SetBool("IsCrouching", false);

        // Возвращаем коллайдер в исходное состояние.
        _playerBrain.Collider.size = _playerBrain.OriginalColliderSize;
        _playerBrain.Collider.offset = _playerBrain.OriginalColliderOffset;
    }
}
