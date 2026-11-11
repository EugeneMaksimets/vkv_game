using System;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private readonly Enemy _enemy;
    private readonly EnemyMovement _movement;
    private readonly EnemyTarget _target;
    private readonly EnemyAnimation _animation;

    private bool _damageAttempted;
    private bool _attackCompleted;

    public event Action AttackCompleted;

    public EnemyAttackState(
        Enemy enemy,
        EnemyMovement movement,
        EnemyTarget target,
        EnemyAnimation animation)
    {
        _enemy = enemy;
        _movement = movement;
        _target = target;
        _animation = animation;
    }

    public void Enter()
    {
        _damageAttempted = false;
        _attackCompleted = false;

        _animation.AttackHit += OnAttackHit;
        _animation.AttackFinished += OnAttackFinished;

        _movement.Stop();
        _movement.FaceTarget(_target.TargetTransform);

        _animation.PlayMoveAnimation(false);
        _animation.PlayAttackAnimation();
    }

    public void UpdateState()
    {
        
    }

    public void Exit()
    {
        _animation.AttackHit -= OnAttackHit;
        _animation.AttackFinished -= OnAttackFinished;
    }

    private void OnAttackHit()
    {
        if (_damageAttempted)
            return;

        _damageAttempted = true;
        _enemy.TryAttack();
    }

    private void OnAttackFinished()
    {
        if (_attackCompleted)
            return;

        _attackCompleted = true;
        AttackCompleted?.Invoke();
    }
}