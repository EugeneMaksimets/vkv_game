using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteraction))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerBrain : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private bool _isJoystickActive;

    [Header("Components")]
    public Rigidbody2D Rb { get; private set; }
    public PlayerAnimation Animation { get; private set; }
    public BoxCollider2D Collider { get; private set; }
    public PlayerInteraction Interaction { get; private set; }
    public PlayerHealth Health { get; private set; }

    public DamageInfo LastDamageInfo { get; private set; }

    [Header("Input")]
    [System.Obsolete(
        "Use PlayerMovement.HorizontalInput instead. " +
        "This property will be removed during state refactoring."
    )]
    public float HorizontalInput => _playerMovement.HorizontalInput;

    public bool IsGrounded => _playerMovement.IsGrounded;
    public float JumpForce => _playerMovement.JumpForce;

    public bool IsJoystickActive
    {
        get => _isJoystickActive;
        set => _isJoystickActive = value;
    }

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
        _playerMovement = GetComponent<PlayerMovement>();

        Rb = GetComponent<Rigidbody2D>();
        Animation = GetComponent<PlayerAnimation>();
        Collider = GetComponent<BoxCollider2D>();
        Interaction = GetComponent<PlayerInteraction>();
        Health = GetComponent<PlayerHealth>();


        StateMachine = new PlayerStateMachine();

        StateFactory = new PlayerStateFactory(this);
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
    }

    private void OnDisable()
    {
        Health.Damaged -= HandleDamaged;
        Health.Died -= HandleDied;
        Health.Revived -= HandleRevived;
    }

    private void Start()
    {
        StateMachine.Initialize(
            Health.IsDead ? DieState : IdleState
        );
    }

    private void Update()
    {
        // Animator.SetBool("IsGrounded", IsGrounded);
        Animation.SetGrounded(IsGrounded);

        HandleMovementStateTransition();

        StateMachine.Update();

    }

    #region Input API

    public void SetHorizontalInput(float input)
    {
        if (IsControlLocked)
        {
            _playerMovement.SetHorizontalInput(0f);
            return;
        }

        _playerMovement.SetHorizontalInput(input);
        
    }

    public void SetMoveRight() => SetHorizontalInput(1f);

    public void SetMoveLeft() => SetHorizontalInput(-1f);

    public void StopMoving() => SetHorizontalInput(0f);

    #endregion

    #region State Transitions

    private void HandleMovementStateTransition()
    {
        if (IsControlLocked || !IsGrounded)
            return;

        IPlayerState currentState = StateMachine.CurrentState;

        float horizontalInput = _playerMovement.HorizontalInput;

        if (currentState == IdleState && Mathf.Abs(horizontalInput) > 0.1f)
        {
            StateMachine.ChangeState(RunState);
        }
        else if (currentState == RunState && Mathf.Abs(horizontalInput) <= 0.1f)
        {
            StateMachine.ChangeState(IdleState);
        }
    }

    public void Jump()
    {
        if (IsControlLocked || !CanJump())
            return;

        _playerMovement.Jump();

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

        _playerMovement.SetCrouching(true);
    }

    public void StandUp()
    {
        if (StateMachine.CurrentState != CrouchState)
            return;

        _playerMovement.SetCrouching(false);

        if (Mathf.Abs(_playerMovement.HorizontalInput) > 0.1f)
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

    #region Damage

    public void ApplyHitKnockback(DamageInfo damageInfo)
    {
        float duration = Mathf.Min(
            damageInfo.KnockbackDuration,
            _getDamageDuration
        );

        _playerMovement.ApplyKnockback(
            damageInfo.SourcePosition,
            damageInfo.KnockbackSpeed,
            duration
        );
    }

    private void HandleDamaged(
        PlayerHealth health,
        DamageInfo damageInfo)
    {
        LastDamageInfo = damageInfo;

        if (health.CurrentHealth > 0f)
        {
            StopMoving();
            _playerMovement.SetCrouching(false);
            StateMachine.ChangeState(GetDamageState);
        }
    }

    private void HandleDied(PlayerHealth health)
    {
        StopMoving();
        _playerMovement.SetCrouching(false);
        _isJoystickActive = false;
        StateMachine.ChangeState(DieState);
    }

    private void HandleRevived(PlayerHealth health)
    {
        StopMoving();
        _playerMovement.SetCrouching(false);
        _isJoystickActive = false;
        StateMachine.ChangeState(IdleState);
    }

    #endregion

    #region Facing Direction


    #endregion

    private void OnValidate()
    {
        _getDamageDuration = Mathf.Max(
            0.01f,
            _getDamageDuration
        );
    }
}