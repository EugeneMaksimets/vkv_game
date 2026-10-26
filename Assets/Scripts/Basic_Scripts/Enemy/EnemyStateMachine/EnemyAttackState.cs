using UnityEngine;

/// <summary>
/// Враг останавливается, запускает анимацию удара и наносит урон в заданный момент.
/// </summary>
public class EnemyAttackState : IEnemyState
{
    private readonly EnemyStateController _controller;
    private float _timer;
    private bool _damageAttempted;

    public EnemyAttackState(EnemyStateController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        if (!_controller.CanMove)
        {
           _controller.Movement.FaceTarget(_controller.Target.Target); 
        }
        _timer = 0f;
        _damageAttempted = false;
        _controller.Movement.Stop();
        _controller.SetMoveAnimation(false);
        _controller.PlayAttackAnimation();
    }

    public void UpdateState()
    {
        _timer += Time.deltaTime;

        if (!_damageAttempted && _timer >= _controller.AttackDamageTime)
        {
            _controller.MeleeAttack.TryAttack();
            _damageAttempted = true;
        }

        if (_timer >= _controller.AttackDuration)
        {
            // Выходим из Attack даже если цель всё ещё в радиусе и cooldown уже готов.
            // Idle проверит условия на следующем кадре и запустит новый цикл атаки.
            // TODO: в будущем удалить когда будет реализация хп на игроке поменятть на:
            // _controller.ChooseIdleWalkOrAttackState);
            _controller.StateMachine.ChangeState(_controller.IdleState);
        }
    }

    public void Exit()
    {
    }
}
