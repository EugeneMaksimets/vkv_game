using System;
using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private readonly PlayerMovement _playerMovement;
    private readonly PlayerAnimation _playerAnimation;
    private readonly float _duration;
    
    private float _timer;
    private bool _isComplated;

    public event Action AttackComplated;

    public PlayerAttackState(PlayerMovement movement, PlayerAnimation animation, float duration = 0.5f)
    {
        _playerMovement = movement;
        _playerAnimation = animation;
        _duration = Mathf.Max(0.01f, duration);
    }

    public void Enter()
    {
        _timer = 0f;
        _isComplated = false;

        _playerAnimation.AttackAnimationFinished += Complete;

        _playerMovement.StopHorizontalMovement();
        _playerAnimation.PlayAttack();
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
        _playerAnimation.AttackAnimationFinished -= Complete;
    }

    private void Complete()
    {
        if(_isComplated)
            return;

        _isComplated = true;
        AttackComplated?.Invoke();
    }
}