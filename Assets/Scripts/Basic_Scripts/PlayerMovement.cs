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
    [SerializeField] private float _minX = -10f; // Minimum X position for the player
    [SerializeField] private float _maxX = 15f; // Maximum X position for the player

    private float _horizontalInput;
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private Animator _animator;

    private Vector3 _originalScale; // Сохраняем исходный размер персонажа
    private Vector2 _originalColliderSize;
    private Vector2 _originalColliderOffset;

    private bool _isGrounded;
    private bool _isCrouching;
    private bool _isJoystickActive = false;
    // Геттер и сеттер в виде стрелочных функций
    public bool IsJoystickActive 
    { 
        get => _isJoystickActive; 
        set => _isJoystickActive = value; 
    }
     

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        
        _originalScale = transform.localScale;
        _originalColliderSize = _collider.size;
        _originalColliderOffset = _collider.offset;
    }

    void Update()
    {
        // Проверяем, касается ли круг под ногами слоя земли
        if (_groundCheck != null)
        {
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

        }

        // 1. Проверяем клавиатуру (если игрок жмет клавиши на ПК)
        if (Keyboard.current != null && !_isJoystickActive)
        {
            // Проверяем, нажата ли какая-то из клавиш движения вправо
            bool isRightPressed = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed;
            // Проверяем, нажата ли какая-то из клавиш движения влево
            bool isLeftPressed = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed;
            // Проверяем, нажата ли клавиша прыжка
            bool isJumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
            bool isCrouchPressed = Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed;
            bool isStandUp = Keyboard.current.sKey.wasReleasedThisFrame || Keyboard.current.downArrowKey.wasReleasedThisFrame;

            if (isRightPressed)
            {
                SetMoveRight();
            }
            else if (isLeftPressed)
            {
                SetMoveLeft();
            }
            else
            {
                StopMoving();
            }
            if (isJumpPressed && _isGrounded)
            {
                Jump();
            }
            if (isCrouchPressed && _isGrounded)
            {
                Crouch();
            }
            else if (isStandUp)
            {
                StandUp();
            }


        } 


        // Передаем абсолютное значение ввода (от 0 до 1) в Animator в параметр "Speed"
        _animator.SetFloat("Speed", Mathf.Abs(_horizontalInput));

        // Поворот спрайта с сохранением твоего размера (например, 1.2)
        if (_horizontalInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
        }
        else if (_horizontalInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
        }
    }

    void FixedUpdate()
    {
        // Move the player based
        _rb.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, _rb.linearVelocity.y);

        // Clamp the player's position within the level bounds if enabled
        if (_useLevelBounds)
        {
            float clampedX = Mathf.Clamp(transform.position.x, _minX, _maxX);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }

    public void SetMoveRight()
    {
        _horizontalInput = 1f;
    }

    public void SetMoveLeft()
    {
        _horizontalInput = -1f;
    }

    public void StopMoving()
    {
        _horizontalInput = 0f;
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
            _animator.SetTrigger("Jump");
        }

    }

    public void Crouch()
    {
        if (!_isCrouching && _isGrounded)
        {
            _isCrouching = true;
            _animator.SetBool("IsCrouching", true);
            // Изменяем размер коллайдера для приседания
            _collider.size = new Vector2(_originalColliderSize.x, _originalColliderSize.y / 2);
            _collider.offset = new Vector2(_originalColliderOffset.x, _originalColliderOffset.y - (_originalColliderSize.y / 4));
        }
    }

    public void StandUp()
    {
            _isCrouching = false;
            _animator.SetBool("IsCrouching", false);

            // Возвращаем коллайдер в исходное состояние
            _collider.size = _originalColliderSize;
            _collider.offset = _originalColliderOffset;
    }

}
