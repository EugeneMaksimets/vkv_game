using System;

public class EnemyGetDamageState : IEnemyState
{
    private readonly EnemyMovement _movement;
    private readonly EnemyAnimation _animation;

    private bool _completed;

    public event Action DamageStateCompleted;

    public EnemyGetDamageState(
        EnemyMovement movement,
        EnemyAnimation animation)
    {
        _movement = movement;
        _animation = animation;
    }

    public void Enter()
    {
        _completed = false;

        _animation.GetDamageFinished += OnGetDamageFinished;

        _movement.Stop();
        _animation.PlayMoveAnimation(false);
        _animation.PlayGetDamageAnimation();
    }

    public void UpdateState()
    {
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
