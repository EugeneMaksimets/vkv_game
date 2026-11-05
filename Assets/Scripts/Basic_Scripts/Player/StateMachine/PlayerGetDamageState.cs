using UnityEngine;

/// <summary>
/// Короткая реакция игрока на несмертельный урон.
/// </summary>
public class PlayerGetDamageState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;
    private float _timer;

    public PlayerGetDamageState(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }

    public void Enter()
    {
        _timer = 0f;

        // Останавливаем движение.
        _playerBrain.StopMoving();

        // Применяем отбрасывание от источника урона.
        _playerBrain.ApplyHitKnockback(
            _playerBrain.LastDamageInfo
        );

        // Запускаем анимацию получения урона.
        _playerBrain.Animator.SetTrigger("GetDamage");
    }

    public void UpdateState()
    {
        _timer += Time.deltaTime;

        // Смерть обрабатывается отдельно через PlayerHealth.Died.
        if (_playerBrain.Health.IsDead)
            return;

        if (_timer >= _playerBrain.GetDamageDuration)
        {
            _playerBrain.StateMachine.ChangeState(
                _playerBrain.IdleState
            );
        }
    }

    public void Exit()
    {
    }
}