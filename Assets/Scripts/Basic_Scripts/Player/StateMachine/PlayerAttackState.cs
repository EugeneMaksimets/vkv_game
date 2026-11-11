
using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;
    private readonly float _attackDuration;

    private float _attackTimer;
    
    public PlayerAttackState(
        PlayerBrain playerBrain,
        PlayerMovement playerMovement,
        PlayerAnimation playerAnimation,
        float attackDuration)
    {
        _playerBrain = playerBrain;
        _playerMovement = playerMovement;
        _playerAnimation = playerAnimation;
        _attackDuration = attackDuration;
    }

    public void Enter()
    {
        _playerMovement.StopHorizontalMovement();
        _playerAnimation.PlayAttack();

        _attackTimer = _attackDuration;
    }

    public void UpdateState()
    {
        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0f)
        {
            if (Mathf.Abs(_playerMovement.HorizontalInput) > 0.1f)
            {
                _playerBrain.StateMachine.ChangeState(
                    _playerBrain.RunState
                );
            }
            else
            {
                _playerBrain.StateMachine.ChangeState(
                    _playerBrain.IdleState
                );
            }
        }
    }

    public void Exit() { }
}
