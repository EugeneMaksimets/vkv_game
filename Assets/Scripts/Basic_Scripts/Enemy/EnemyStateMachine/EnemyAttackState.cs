using System;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private readonly Enemy _enemy;
    private readonly EnemyMovement _movement;
    private readonly EnemyTarget _target;
    private readonly EnemyAnimation _animation;
    private readonly float _fallbackTimeout;

    private bool _damageAttempted;
    private bool _attackCompleted;
    private float _timer;

    public event Action AttackCompleted;

    public EnemyAttackState(
        Enemy enemy,
        EnemyMovement movement,
        EnemyTarget target,
        EnemyAnimation animation,
        float fallbackTimeout = 2f)
    {
        _enemy = enemy;
        _movement = movement;
        _target = target;
        _animation = animation;
        _fallbackTimeout = fallbackTimeout;
    }

    public void Enter()
    {
        _damageAttempted = false;
        _attackCompleted = false;
        _timer = 0f;

        _animation.AttackHit += OnAttackHit;
        _animation.AttackFinished += OnAttackFinished;

        _enemy.StartAttackCooldown();

        _movement.Stop();
        _movement.FaceTarget(_target.TargetTransform);

        _animation.PlayMoveAnimation(false);
        _animation.PlayAttackAnimation();
    }

    public void UpdateState()
    {
        if (_attackCompleted)
            return;

        _timer += Time.deltaTime;
        if (_timer >= _fallbackTimeout)
        {
            Debug.LogWarning($"{nameof(EnemyAttackState)}: Animation event AttackFinished missed, triggered fallback timeout.");
            OnAttackFinished();
        }
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
        
        if (!_damageAttempted)
        {
            Debug.LogWarning($"{nameof(EnemyAttackState)}: Animation finished without triggering AttackHit event.");
        }

        _attackCompleted = true;
        AttackCompleted?.Invoke();
    }
}
