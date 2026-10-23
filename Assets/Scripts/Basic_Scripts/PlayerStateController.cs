
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerStateController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement _playerMovement;

    public Rigidbody2D Rb { get; private set; }
    public Animator Animator { get; private set; }
    public BoxCollider2D Collider { get; private set; }
    public PlayerMovement PlayerMovement => _playerMovement;

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
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<BoxCollider2D>();

        if (_playerMovement == null)
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
        // HandleKeyboardInput();
        Animator.SetBool("IsGrounded", IsGrounded);

        StateMachine.Update();

        UpdateFacingDirection();
    }

    private void HandleKeyboardInput()
    {
        if (_isJoystickActive || Keyboard.current == null)
            return;

        var keyboard = Keyboard.current;

        bool right =
            keyboard.dKey.isPressed ||
            keyboard.rightArrowKey.isPressed;

        bool left =
            keyboard.aKey.isPressed ||
            keyboard.leftArrowKey.isPressed;

        bool crouch =
            keyboard.sKey.isPressed ||
            keyboard.downArrowKey.isPressed;

        bool crouchReleased =
            keyboard.sKey.wasReleasedThisFrame ||
            keyboard.downArrowKey.wasReleasedThisFrame;

        if (right && !left)
            SetMoveRight();
        else if (left && !right)
            SetMoveLeft();
        else
            StopMoving();

        if (keyboard.spaceKey.wasPressedThisFrame)
            Jump();

        if (crouch)
            Crouch();
        else if (crouchReleased)
            StandUp();

        if (keyboard.fKey.wasPressedThisFrame)
            Attack();
    }

    private void UpdateFacingDirection()
    {
        if (HorizontalInput > 0f)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(_originalScale.x),
                _originalScale.y,
                _originalScale.z
            );
        }
        else if (HorizontalInput < 0f)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(_originalScale.x),
                _originalScale.y,
                _originalScale.z
            );
        }
    }

    // Input API: клавиатура и мобильный джойстик

    public void SetHorizontalInput(float input)
    {
        HorizontalInput = Mathf.Clamp(input, -1f, 1f);
        _playerMovement.SetHorizontalInput(HorizontalInput);
    }

    public void SetMoveRight() => SetHorizontalInput(1f);

    public void SetMoveLeft() => SetHorizontalInput(-1f);

    public void StopMoving() => SetHorizontalInput(0f);

    // State transitions

    public void Jump()
    {
        if (IsGrounded &&
            StateMachine.CurrentState != CrouchState &&
            StateMachine.CurrentState != AttackState &&
            StateMachine.CurrentState != JumpState)
        {
            StateMachine.ChangeState(JumpState);
        }
    }

    public void Crouch()
    {
        if (IsGrounded &&
            StateMachine.CurrentState != JumpState &&
            StateMachine.CurrentState != AttackState &&
            StateMachine.CurrentState != CrouchState)
        {
            StateMachine.ChangeState(CrouchState);
        }
    }

    public void StandUp()
    {
        if (StateMachine.CurrentState != CrouchState)
            return;

        if (Mathf.Abs(HorizontalInput) > 0.1f)
            StateMachine.ChangeState(RunState);
        else
            StateMachine.ChangeState(IdleState);
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
