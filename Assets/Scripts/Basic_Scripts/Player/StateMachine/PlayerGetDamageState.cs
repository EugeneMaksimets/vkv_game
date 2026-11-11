using UnityEngine;

public class PlayerGetDamageState : IPlayerState
{
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;

    private DamageInfo _damageInfo;

    public PlayerGetDamageState(PlayerMovement movement, PlayerAnimation animation)
    {
        _playerMovement = movement;
        _playerAnimation = animation;
    }

    public void SetDamageInfo(DamageInfo damageInfo)
    {
        _damageInfo = damageInfo;
    }

    public void Enter()
    {
        _playerMovement.StopHorizontalMovement();

        _playerMovement.ApplyHitKnockback(
            _damageInfo,
            _damageInfo.KnockbackDuration
        );

        _playerAnimation.PlayGetDamage();
    }

    public void UpdateState() { }

    public void Exit() { }
}