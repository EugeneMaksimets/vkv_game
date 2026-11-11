using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    [Header("Animator Parameters")]
    [SerializeField] private string _speedParameter = "Speed";
    [SerializeField] private string _attackTrigger = "Attack";
    [SerializeField] private string _getDamageTrigger = "GetDamage";
    [SerializeField] private string _dieTrigger = "Die";

    public event Action AttackHit;
    public event Action AttackFinished;
    public event Action GetDamageFinished;

    private Animator _animator;

    private int _speedParameterHash;
    private int _attackTriggerHash;
    private int _getDamageTriggerHash;
    private int _dieTriggerHash;

    private bool _isInitialized;

    private void Awake()
    {
        EnsureInitialized(); 
    }

    private void EnsureInitialized()
    {
        if (_isInitialized)
            return;
        _animator = GetComponent<Animator>();

        _speedParameterHash = Animator.StringToHash(_speedParameter);
        _attackTriggerHash = Animator.StringToHash(_attackTrigger);
        _getDamageTriggerHash = Animator.StringToHash(_getDamageTrigger);
        _dieTriggerHash = Animator.StringToHash(_dieTrigger);

        _isInitialized = true;
    }

    public void PlayMoveAnimation(bool isMoving)
    {
        EnsureInitialized();
        _animator.SetFloat(_speedParameterHash, isMoving ? 1f : 0f);
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger(_attackTriggerHash);
    }

    public void PlayGetDamageAnimation()
    {
        _animator.SetTrigger(_getDamageTriggerHash);
    }

    public void PlayDieAnimation()
    {
        _animator.SetTrigger(_dieTriggerHash);
    }

    public void OnAttackHit()
    {
        AttackHit?.Invoke();
    }

    public void OnAttackFinished()
    {
        AttackFinished?.Invoke();
    }

        public void OnGetDamageFinished()
    {
        GetDamageFinished?.Invoke();
    }
}