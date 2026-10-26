using UnityEngine;

/// <summary>
/// На короткое время прерывает действия врага после получения урона.
/// </summary>
public class EnemyGetDamageState : IEnemyState
{
    private readonly EnemyStateController _controller;
    private float _timer;

    public EnemyGetDamageState(EnemyStateController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        _timer = 0f;
        _controller.Movement.Stop();
        _controller.SetMoveAnimation(false);
        _controller.PlayGetDamageAnimation();
    }

    public void UpdateState()
    {
        _timer += Time.deltaTime;

        if (_timer >= _controller.GetDamageDuration)
            _controller.ChooseIdleWalkOrAttackState();
    }

    public void Exit()
    {
    }
}
