
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
        _playerBrain.Animation.SetCrouching(true);

        _playerBrain.Collider.size = new Vector2(
            _playerBrain.OriginalColliderSize.x,
            _playerBrain.OriginalColliderSize.y / 2
        );

        _playerBrain.Collider.offset = new Vector2(
            _playerBrain.OriginalColliderOffset.x,
            _playerBrain.OriginalColliderOffset.y -
            (_playerBrain.OriginalColliderSize.y / 4)
        );
    }

    public void UpdateState()
    {
        _playerBrain.Animation.SetSpeed(
            Mathf.Abs(_playerBrain.HorizontalInput)
        );
    }

    public void Exit()
    {
        _playerBrain.Animation.SetCrouching(false);

        _playerBrain.Collider.size =
            _playerBrain.OriginalColliderSize;

        _playerBrain.Collider.offset =
            _playerBrain.OriginalColliderOffset;
    }
}
