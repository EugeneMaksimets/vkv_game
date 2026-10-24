using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerStateController : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    public Rigidbody2D Rb { get; private set; }
    public Animator Animator { get; private set; }
    public BoxCollider2D Collider { get; private set; }

    [Header("Input")]
    public float HorizontalInput { get; private set; }

    public bool IsGrounded => _playerMovement.IsGrounded;
    public float JumpForce => _playerMovement.JumpForce;

    public Vector2 OriginalColliderSize { get; private set; }
    public Vector2 OriginalColliderOffset { get; private set; }

    private Vector3 _originalScale;
    private bool _isJoystickActive;

    public bool IsJoystickActive
    {
        get => _isJoystickActive;
        set => _isJoystickActive = value;
    }

    [Header("State Machine")]
    public PlayerStateMachine StateMachine { get; private set; }
    internal PlayerIdleState IdleState { get; private set; }
    internal PlayerRunState RunState { get; private set; }
    internal PlayerJumpState JumpState { get; private set; }
    internal PlayerCrouchState CrouchState { get; private set; }
    internal PlayerAttackState AttackState { get; private set; }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<BoxCollider2D>();
        _playerMovement = GetComponent<PlayerMovement>();

        OriginalColliderSize = Collider.size;
        OriginalColliderOffset = Collider.offset;
        _originalScale = transform.localScale;

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this);
        RunState = new PlayerRunState(this);
        JumpState = new PlayerJumpState(this);
        CrouchState = new PlayerCrouchState(this);
        AttackState = new PlayerAttackState(this);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Animator.SetBool("IsGrounded", IsGrounded);

        StateMachine.Update();

        UpdateFacingDirection();
    }



    private void UpdateFacingDirection()
    {
        if (HorizontalInput > 0f)
        {
            Vector3 newScale = new Vector3(
                Mathf.Abs(_originalScale.x),
                _originalScale.y,
                _originalScale.z
            );
            if (transform.localScale != newScale)
                transform.localScale = newScale;
        }
        else if (HorizontalInput < 0f)
        {
            Vector3 newScale = new Vector3(
                -Mathf.Abs(_originalScale.x),
                _originalScale.y,
                _originalScale.z
            );
            if (transform.localScale != newScale)
                transform.localScale = newScale;
        }
    }

    // Input API: клавиатура и мобильный джойстик

    public void SetHorizontalInput(float input)
    {
        HorizontalInput = input;
        _playerMovement.SetHorizontalInput(HorizontalInput);
    }

    public void SetMoveRight() => SetHorizontalInput(1f);

    public void SetMoveLeft() => SetHorizontalInput(-1f);

    public void StopMoving() => SetHorizontalInput(0f);

    // State transitions

    public void Jump()
    {
        if (CanJump())
        {
            StateMachine.ChangeState(JumpState);
        }
    }

    private bool CanJump()
    {
        return IsGrounded &&
               StateMachine.CurrentState != CrouchState &&
               StateMachine.CurrentState != AttackState &&
               StateMachine.CurrentState != JumpState;
    }

    public void Crouch()
    {
        if (IsGrounded &&
            StateMachine.CurrentState != JumpState &&
            StateMachine.CurrentState != AttackState &&
            StateMachine.CurrentState != CrouchState)
        {
            StateMachine.ChangeState(CrouchState);
            _playerMovement.SetCrouching(true);
        }
    }

    public void StandUp()
    {
        if (StateMachine.CurrentState != CrouchState)
            return;

        if (Mathf.Abs(HorizontalInput) > 0.1f)
        {
            StateMachine.ChangeState(RunState);
            _playerMovement.SetCrouching(false);
        }
        else
        {
            StateMachine.ChangeState(IdleState);
            _playerMovement.SetCrouching(false);
        }
    }

    public void Attack()
    {
        if (IsGrounded &&
            StateMachine.CurrentState != CrouchState &&
            StateMachine.CurrentState != RunState &&
            StateMachine.CurrentState != AttackState &&
            StateMachine.CurrentState != JumpState)
        {
            StateMachine.ChangeState(AttackState);
        }
    }
}
