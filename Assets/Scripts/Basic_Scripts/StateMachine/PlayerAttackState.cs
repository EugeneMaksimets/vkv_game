using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private PlayerStateController _playerStateControler;
    private float _attackTimer;
    private float _attackDuration = 0.5f;

    public PlayerAttackState(PlayerStateController playerStateController)
    {
        _playerStateControler = playerStateController;
    }

    public void Enter()
    {
        _playerStateControler.StopMoving();
        _playerStateControler.Animator.SetTrigger("Attack");
        _attackTimer = _attackDuration;

        // Здесь же можно вызвать метод нанесения урона
    }

    public void UpdateState()
    {
        _attackTimer -= Time.deltaTime;

        // Когда время анимации/удара истекло, возвращаемся в Idle или Run
        if (_attackTimer <= 0f)
        {
            if (Mathf.Abs(_playerStateControler.HorizontalInput) > 0.1f)
            {
                _playerStateControler.StateMachine.ChangeState(_playerStateControler.RunState);
            }
            else
            {
                _playerStateControler.StateMachine.ChangeState(_playerStateControler.IdleState);
            }
        }
    }

    public void Exit() {}

}