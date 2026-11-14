using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    private static readonly int SpeedParameterHash = Animator.StringToHash("Speed");
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
    private static readonly int GetDamageTriggerHash = Animator.StringToHash("GetDamage");
    private static readonly int DieTriggerHash = Animator.StringToHash("Die");
    private Animator _animator;

    public event Action AttackHit;
    public event Action AttackFinished;
    public event Action GetDamageFinished;

    private void Awake() =>
        _animator = GetComponent<Animator>();

    public void PlayMoveAnimation(bool isMoving) =>
        _animator.SetFloat(SpeedParameterHash, isMoving ? 1f : 0f);

    public void PlayAttackAnimation() =>
        _animator.SetTrigger(AttackTriggerHash);
        
    public void PlayGetDamageAnimation() =>
        _animator.SetTrigger(GetDamageTriggerHash);

    public void PlayDieAnimation() =>
        _animator.SetTrigger(DieTriggerHash);

    public void OnAttackHit() => AttackHit?.Invoke();

    public void OnAttackFinished() => AttackFinished?.Invoke();

    public void OnGetDamageFinished() => GetDamageFinished?.Invoke();
}