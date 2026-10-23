using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Level Bounds")]
    [SerializeField] private bool _useLevelBounds = true;
    [SerializeField] private float _minX = -10f;
    [SerializeField] private float _maxX = 15f;

    // Свойства для доступа из состояний
    public Rigidbody2D Rb { get; private set; }
    public Animator Animator { get; private set; }
    public BoxCollider2D Collider { get; private set; }
    public float HorizontalInput { get; private set; }
    public bool IsGrounded { get; private set; }
    public float JumpForce => _jumpForce;

    private Vector3 _originalScale;
    public Vector2 OriginalColliderSize { get; private set; }
    public Vector2 OriginalColliderOffset { get; private set; }

    private bool _isJoystickActive = false;
    public bool IsJoystickActive 
    { 
        get => _isJoystickActive; 
        set => _isJoystickActive = value; 
    }

    // State Machine и ссылки на состояния
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }

    void Awake() {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<BoxCollider2D>();

        // Инициализируем State Machine
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this);
        RunState = new PlayerRunState(this);
        JumpState = new PlayerJumpState(this);
        CrouchState = new PlayerCrouchState(this);
    }
    void Start()
    {
        _originalScale = transform.localScale;
        OriginalColliderSize = Collider.size;
        OriginalColliderOffset = Collider.offset;

        // Стартуем в состоянии покоя
        StateMachine.Initialize(IdleState);
    }

    void Update()
    {
        // 1. Проверяем землю
        if (_groundCheck != null)
        {
            IsGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
            Animator.SetBool("IsGrounded", IsGrounded);
        }

        // 2. Считываем клавиатуру (если не активен джойстик)
        if (Keyboard.current != null && !_isJoystickActive)
        {
            bool isRightPressed = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed;
            bool isLeftPressed = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed;
            bool isJumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
            bool isCrouchPressed = Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed;
            bool isStandUp = Keyboard.current.sKey.wasReleasedThisFrame || Keyboard.current.downArrowKey.wasReleasedThisFrame;

            if (isRightPressed) SetMoveRight();
            else if (isLeftPressed) SetMoveLeft();
            else StopMoving();

            if (isJumpPressed) Jump();

            if (isCrouchPressed) Crouch();
            else if (isStandUp) StandUp();
        }

        // 3. Обновляем логику текущего состояния State Machine
        StateMachine.Update();

        // 4. Поворот спрайта
        if (HorizontalInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
        }
        else if (HorizontalInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
        }
    }

    void FixedUpdate()
    {
        // Скорость передвижения снижается в два раза, если мы сидим
        float currentSpeed = (StateMachine.CurrentState == CrouchState) ? _moveSpeed * 0.5f : _moveSpeed;
        Rb.linearVelocity = new Vector2(HorizontalInput * currentSpeed, Rb.linearVelocity.y);

        // Ограничения уровня
        if (_useLevelBounds)
        {
            float clampedX = Mathf.Clamp(transform.position.x, _minX, _maxX);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }

    // Методы управления вводом (вызываются с клавиатуры или джойстика)
    public void SetMoveRight() => HorizontalInput = 1f;
    public void SetMoveLeft() => HorizontalInput = -1f;
    public void StopMoving() => HorizontalInput = 0f;

    public void Jump()
    {
        // Прыгать можно только с земли и если мы не сидим
        if (IsGrounded && StateMachine.CurrentState != CrouchState)
        {
            StateMachine.ChangeState(JumpState);
        }
    }

    public void Crouch()
    {
        // Приседать можно только на земле и если мы не в прыжке
        if (IsGrounded && StateMachine.CurrentState != JumpState && StateMachine.CurrentState != CrouchState)
        {
            StateMachine.ChangeState(CrouchState);
        }
    }

    public void StandUp()
    {
        // Встать можно только из состояния приседания
        if (StateMachine.CurrentState == CrouchState)
        {
            // Возвращаемся в Idle или сразу в Run, если зажата клавиша движения
            if (Mathf.Abs(HorizontalInput) > 0.1f)
                StateMachine.ChangeState(RunState);
            else
                StateMachine.ChangeState(IdleState);
        }
    }
}