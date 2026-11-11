using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteraction))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerBrain : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField, Min(0.01f)]
    private float _attackDuration = 0.5f;

    private float _attackTimer;

    [Header("Components")]
    public PlayerAnimation Animation { get; private set; }
    public PlayerInteraction Interaction { get; private set; }
    public PlayerHealth Health { get; private set; }

    public DamageInfo LastDamageInfo { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public bool IsJoystickActive { get; set; }

    public bool IsGrounded => Movement.IsGrounded;

    [Header("Health State Settings")]
    [SerializeField] private float _getDamageDuration = 0.25f;

    public float GetDamageDuration => _getDamageDuration;

    
    [Header("State Machine")]
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerStateFactory StateFactory { get; private set; }


    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerInteractState InteractState { get; private set; }
    public PlayerGetDamageState GetDamageState { get; private set; }
    public PlayerDieState DieState { get; private set; }

    private float _getDamageTimer;

    public bool IsControlLocked
    {
        get
        {
            if (Health == null || Health.IsDead)
                return true;

            return StateMachine != null &&
                   (StateMachine.CurrentState == GetDamageState ||
                    StateMachine.CurrentState == DieState);
        }
    }

    private void Awake()
    {
        Animation = GetComponent<PlayerAnimation>();
        Interaction = GetComponent<PlayerInteraction>();
        Health = GetComponent<PlayerHealth>();
        Movement = GetComponent<PlayerMovement>();


        StateMachine = new PlayerStateMachine();

        StateFactory = new PlayerStateFactory(Movement, Animation, Interaction);
        StateFactory.CreateStates();

        IdleState = StateFactory.IdleState;
        RunState = StateFactory.RunState;
        JumpState = StateFactory.JumpState;
        CrouchState = StateFactory.CrouchState;
        AttackState = StateFactory.AttackState;
        InteractState = StateFactory.InteractState;
        GetDamageState = StateFactory.GetDamageState;
        DieState = StateFactory.DieState;
    }

    private void OnEnable()
    {
        Health.Damaged += HandleDamaged;
        Health.Died += HandleDied;
        Health.Revived += HandleRevived;

        Animation.InteractionAnimationFinished += HandleInteractionAnimationFinished;
    }

    private void OnDisable()
    {
        Health.Damaged -= HandleDamaged;
        Health.Died -= HandleDied;
        Health.Revived -= HandleRevived;

        Animation.InteractionAnimationFinished -= HandleInteractionAnimationFinished;
    }

    private void Start()
    {
        StateMachine.Initialize(
            Health.IsDead ? DieState : IdleState
        );
    }

    private void Update()
    {
        Animation.SetGrounded(IsGrounded);

        HandleMovementStateTransition();
        HandleAttackStateTransition();
        HandleGetDamageStateTransition();

        StateMachine.Update();

    }

    #region Input API

    public void SetHorizontalInput(float input)
    {
        if (IsControlLocked || StateMachine.CurrentState == AttackState)
        {
            Movement.SetHorizontalInput(0f);
            return;
        }

        Movement.SetHorizontalInput(input);
        
    }

    public void SetMoveRight() => SetHorizontalInput(1f);

    public void SetMoveLeft() => SetHorizontalInput(-1f);

    public void StopMoving() => SetHorizontalInput(0f);

    #endregion

    #region State Transitions

    private void HandleMovementStateTransition()
    {
        if (IsControlLocked)
            return;

        IPlayerState currentState = StateMachine.CurrentState;

        float horizontalInput = Movement.HorizontalInput;

        if(currentState == JumpState)
        {
            if(!JumpState.HasLeftGround || !Movement.IsGrounded || Movement.VerticalVelocity > 0.01f)
                return;
        
            if(Math.Abs(horizontalInput) > 0.1f)
                StateMachine.ChangeState(RunState);
            else
                StateMachine.ChangeState(IdleState);

            return;
        }

        if (currentState == IdleState && Mathf.Abs(horizontalInput) > 0.1f)
            StateMachine.ChangeState(RunState);
        else if (currentState == RunState && Mathf.Abs(horizontalInput) <= 0.1f)
            StateMachine.ChangeState(IdleState);
    }

    public void Jump()
    {
        if (IsControlLocked || !CanJump())
            return;

        Movement.Jump();

        StateMachine.ChangeState(JumpState);
    }

    private bool CanJump()
    {
        return IsGrounded &&
               StateMachine.CurrentState != CrouchState &&
               StateMachine.CurrentState != AttackState &&
               StateMachine.CurrentState != JumpState &&
               StateMachine.CurrentState != GetDamageState &&
               StateMachine.CurrentState != DieState;
    }

    public void Crouch()
    {
        if (IsControlLocked ||
            !IsGrounded ||
            StateMachine.CurrentState == JumpState ||
            StateMachine.CurrentState == AttackState ||
            StateMachine.CurrentState == CrouchState ||
            StateMachine.CurrentState == GetDamageState ||
            StateMachine.CurrentState == DieState)
        {
            return;
        }

        StateMachine.ChangeState(CrouchState);

    }

    public void StandUp()
    {
        if (StateMachine.CurrentState != CrouchState)
            return;

        if (Mathf.Abs(Movement.HorizontalInput) > 0.1f)
        {
            StateMachine.ChangeState(RunState);
        }
        else
        {
            StateMachine.ChangeState(IdleState);
        }
    }

    public void Attack()
    {
        if (IsControlLocked ||
            !IsGrounded ||
            StateMachine.CurrentState == CrouchState ||
            StateMachine.CurrentState == RunState ||
            StateMachine.CurrentState == AttackState ||
            StateMachine.CurrentState == JumpState ||
            StateMachine.CurrentState == GetDamageState ||
            StateMachine.CurrentState == DieState)
        {
            return;
        }

        _attackTimer = 0f;
        StateMachine.ChangeState(AttackState);
    }

    public void Interact()
    {
        if (IsControlLocked)
            return;

        bool hasInteractable =
            Interaction != null &&
            Interaction.HasInteractable;

        if (hasInteractable)
        {
            StateMachine.ChangeState(InteractState);
        }
        else
        {
            Debug.Log("Nothing to use");
        }
    }

    #endregion


    private void HandleAttackStateTransition()
    {
        if (StateMachine.CurrentState != AttackState)
            return;

        _attackTimer += Time.deltaTime;

        if (_attackTimer < _attackDuration)
            return;

        if (Movement.HorizontalInput != 0f)
            StateMachine.ChangeState(RunState);
        else
            StateMachine.ChangeState(IdleState);
    }

    private void HandleInteractionAnimationFinished()
    {
        if (StateMachine.CurrentState != InteractState)
            return;

        StateMachine.ChangeState(IdleState);
    }

    #region Damage

    private void HandleGetDamageStateTransition()
    {
        if (StateMachine.CurrentState != GetDamageState)
            return;

        if (Health.IsDead)
            return;

        _getDamageTimer += Time.deltaTime;

        if (_getDamageTimer >= _getDamageDuration)
        {
            StateMachine.ChangeState(IdleState);
        }
    }

    private void HandleDamaged(
        PlayerHealth health,
        DamageInfo damageInfo)
    {
        LastDamageInfo = damageInfo;

        if (health.CurrentHealth <= 0f)
            return;

        StopMoving();
        Movement.SetCrouching(false);

        _getDamageTimer = 0f;

        GetDamageState.SetDamageInfo(damageInfo);

        StateMachine.ChangeState(GetDamageState);
    }

    private void HandleDied(PlayerHealth health)
    {
        StopMoving();
        Movement.SetCrouching(false);
        IsJoystickActive = false;
        StateMachine.ChangeState(DieState);
    }

    private void HandleRevived(PlayerHealth health)
    {
        StopMoving();
        Movement.SetCrouching(false);
        IsJoystickActive = false;
        StateMachine.ChangeState(IdleState);
    }

    #endregion

    private void OnValidate()
    {
        _getDamageDuration = Mathf.Max(
            0.01f,
            _getDamageDuration
        );
    }
}