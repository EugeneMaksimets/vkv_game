using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    public event Action InteractionAnimationFinished;
    private static readonly int IsGroundedHash =
        Animator.StringToHash("IsGrounded");

    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int IsCrouchingHash =
        Animator.StringToHash("IsCrouching");

    private static readonly int JumpHash =
        Animator.StringToHash("Jump");

    private static readonly int AttackHash =
        Animator.StringToHash("Attack");

    private static readonly int PickUpItemHash =
        Animator.StringToHash("PickUpItem");

    private static readonly int GetDamageHash =
        Animator.StringToHash("GetDamage");

    private static readonly int DieHash =
        Animator.StringToHash("Die");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetGrounded(bool isGrounded)
    {
        _animator.SetBool(IsGroundedHash, isGrounded);
    }

    public void SetSpeed(float speed)
    {
        _animator.SetFloat(SpeedHash, speed);
    }

    public void SetCrouching(bool isCrouching)
    {
        _animator.SetBool(IsCrouchingHash, isCrouching);
    }

    public void PlayJump()
    {
        _animator.SetTrigger(JumpHash);
    }

    public void PlayAttack()
    {
        _animator.SetTrigger(AttackHash);
    }

    public void PlayPickUpItem()
    {
        _animator.SetTrigger(PickUpItemHash);
    }

    public void PlayGetDamage()
    {
        _animator.SetTrigger(GetDamageHash);
    }

    public void PlayDie()
    {
        _animator.SetTrigger(DieHash);
    }

    public void OnInteractionAnimationFinished()
    {
        InteractionAnimationFinished?.Invoke();
    }
}
