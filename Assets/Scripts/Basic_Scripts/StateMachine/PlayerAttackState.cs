using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private PlayerStateController _player;
    private float _attackTimer;
    private float _attackDuration = 0.5f;

    public PlayerAttackState(PlayerStateController player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.StopMoving();
        _player.Animator.SetTrigger("Attack");
        _attackTimer = _attackDuration;

        // Здесь же можно вызвать метод нанесения урона
    }

    public void UpdateState()
    {
        _attackTimer -= Time.deltaTime;

        // Когда время анимации/удара истекло, возвращаемся в Idle или Run
        if (_attackTimer <= 0f)
        {
            if (Mathf.Abs(_player.HorizontalInput) > 0.1f)
            {
                _player.StateMachine.ChangeState(_player.RunState);
            }
            else
            {
                _player.StateMachine.ChangeState(_player.IdleState);
            }
        }
    }

    public void Exit() {}

}