
using UnityEngine;

public class PlayerGetDamageState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;
    private float _timer;

    public PlayerGetDamageState(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }

    public void Enter()
    {
        _timer = 0f;

        _playerBrain.StopMoving();

        _playerBrain.ApplyHitKnockback(
            _playerBrain.LastDamageInfo
        );

        _playerBrain.Animation.PlayGetDamage();
    }

    public void UpdateState()
    {
        _timer += Time.deltaTime;

        if (_playerBrain.Health.IsDead)
            return;

        if (_timer >= _playerBrain.GetDamageDuration)
        {
            _playerBrain.StateMachine.ChangeState(
                _playerBrain.IdleState
            );
        }
    }

    public void Exit() { }
}
