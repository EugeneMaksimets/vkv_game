using System;
using UnityEngine;

public class EnemyGetDamageState : IEnemyState
{
    private readonly EnemyMovement _movement;
    private readonly EnemyAnimation _animation;
    private readonly float _fallbackTimeout;

    private bool _completed;
    private float _timer;

    public event Action DamageStateCompleted;

    public EnemyGetDamageState(
        EnemyMovement movement,
        EnemyAnimation animation,
        float fallbackTimeout = 2f)
    {
        _movement = movement;
        _animation = animation;
        _fallbackTimeout = fallbackTimeout;
    }

    public void Enter()
    {
        _completed = false;

        _timer = 0f;

        _animation.GetDamageFinished += OnGetDamageFinished;

        _movement.Stop();
        _animation.PlayMoveAnimation(false);
        _animation.PlayGetDamageAnimation();
    }

    public void UpdateState()
    {
        if(_completed)
            return;

        _timer += Time.deltaTime;
        if(_timer >= _fallbackTimeout)
        {
            Debug.LogWarning($"{nameof(EnemyGetDamageState)}: Animation event GetDamageFinished missed, triggered fallback timeout.");
        
            OnGetDamageFinished();
        }
    }

    public void Exit()
    {
        _animation.GetDamageFinished -= OnGetDamageFinished;
    }

    private void OnGetDamageFinished()
    {
        if (_completed)
            return;

        _completed = true;
        DamageStateCompleted?.Invoke();
    }
}
