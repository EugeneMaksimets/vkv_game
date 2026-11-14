using System;
using UnityEngine;

public class PlayerGetDamageState : IPlayerState
{
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;
    private readonly float _duration;

    private DamageInfo _damageInfo;
    private float _timer;
    private bool _isComplated;

    public event Action DamageStateCompleted;

    public PlayerGetDamageState(PlayerMovement movement, PlayerAnimation animation, float duration = 0.25f)
    {
        _playerMovement = movement;
        _playerAnimation = animation;
        _duration = Mathf.Max(0.01f, duration);
    }

    public void Enter()
    {
        _timer = 0f;
        _isComplated = false;

        _playerAnimation.GetDamageAnimationFinished += Complete;

        _playerMovement.StopHorizontalMovement();

        _playerMovement.ApplyHitKnockback(
            _damageInfo,
            _duration
        );

        _playerAnimation.PlayGetDamage();
    }

    public void UpdateState()
    {
        if(_isComplated)
            return;

        _timer += Time.deltaTime;
        if(_timer >= _duration)
            Complete();
    }

    public void Exit()
    {
        _playerAnimation.GetDamageAnimationFinished -= Complete;
    }

    private void Complete()
    {
        if(_isComplated)
            return;

        _isComplated = true;
        DamageStateCompleted?.Invoke();
    }

    public void SetDamageInfo(DamageInfo damageInfo)
    {
        _damageInfo = damageInfo;
    }

}