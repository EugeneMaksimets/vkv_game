using UnityEngine;

/// <summary>
/// Короткая реакция игрока на несмертельный урон.
/// Состояние запускается через событие PlayerHealth.Damaged,
/// а не напрямую компонентами врага.
/// </summary>
public class PlayerGetDamageState : IPlayerState
{
    private readonly PlayerStateController _playerStateController;
    private float _timer;

    public PlayerGetDamageState(PlayerStateController playerStateController)
    {
        _playerStateController = playerStateController;
    }

    public void Enter()
    {
        _timer = 0f;

        // Останавливаем игрока на время hit reaction.
        // Если до этого было приседание, переход вызовет Exit у PlayerCrouchState
        // и вернёт исходный размер коллайдера.
        _playerStateController.StopMoving();
        _playerStateController.ApplyHitKnockback(_playerStateController.LastDamageInfo);
        _playerStateController.PlayGetDamageAnimation();
    } 

    public void UpdateState()
    {
        _timer += Time.deltaTime;

        // Смерть обрабатывается событием PlayerHealth.Died и переводит в PlayerDieState.
        if (_playerStateController.Health.IsDead)
            return;

        if (_timer >= _playerStateController.GetDamageDuration)
            _playerStateController.StateMachine.ChangeState(_playerStateController.IdleState);
    }

    public void Exit()
    {
    }
}
