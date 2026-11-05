using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;

    private float _attackTimer;
    private readonly float _attackDuration = 0.5f;

    public PlayerAttackState(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }

    public void Enter()
    {
        _playerBrain.StopMoving();

        _playerBrain.Animator.SetTrigger("Attack");

        _attackTimer = _attackDuration;
    }

    public void UpdateState()
    {
        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0f)
        {
            if (Mathf.Abs(_playerBrain.HorizontalInput) > 0.1f)
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

    public void Exit()
    {
    }
}