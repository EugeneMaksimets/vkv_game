using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private PlayerStateController _playerStateController;
    private float _attackTimer;
    private float _attackDuration = 0.5f;

    public PlayerAttackState(PlayerStateController playerStateController)
    {
        _playerStateController = playerStateController;
    }

    public void Enter()
    {
        _playerStateController.StopMoving();
        _playerStateController.Animator.SetTrigger("Attack");
        _attackTimer = _attackDuration;

        // Здесь же можно вызвать метод нанесения урона
    }

    public void UpdateState()
    {
        _attackTimer -= Time.deltaTime;

        // Когда время анимации/удара истекло, возвращаемся в Idle или Run
        if (_attackTimer <= 0f)
        {
            if (Mathf.Abs(_playerStateController.HorizontalInput) > 0.1f)
            {
                _playerStateController.StateMachine.ChangeState(_playerStateController.RunState);
            }
            else
            {
                _playerStateController.StateMachine.ChangeState(_playerStateController.IdleState);
            }
        }
    }

    public void Exit() {}

}