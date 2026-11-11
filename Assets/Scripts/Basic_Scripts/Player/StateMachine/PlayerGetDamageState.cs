
using UnityEngine;

public class PlayerGetDamageState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;
    private readonly PlayerHealth _playerHealth;

    private float _timer;

    public PlayerGetDamageState(
        PlayerBrain playerBrain,
        PlayerMovement playerMovement,
        PlayerAnimation playerAnimation,
        PlayerHealth playerHealth
        )
    {
        _playerBrain = playerBrain;
        _playerMovement = playerMovement;
        _playerAnimation = playerAnimation;
        _playerHealth = playerHealth;
    }

    public void Enter()
    {
        _timer = 0f;

        _playerMovement.StopHorizontalMovement();

        _playerBrain.ApplyHitKnockback(
            _playerBrain.LastDamageInfo
        );

        _playerAnimation.PlayGetDamage();
    }

    public void UpdateState()
    {
        if (_playerHealth.IsDead)
            return;

        _timer += Time.deltaTime;

        if (_timer >= _playerBrain.GetDamageDuration)
        {
            _playerBrain.StateMachine.ChangeState(
                _playerBrain.IdleState
            );
        }
    }

    public void Exit() { }
}
