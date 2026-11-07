
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _crouchSpeedMultiplier = 0.5f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Level Bounds")]
    [SerializeField] private bool _useLevelBounds = true;
    [SerializeField] private float _minX = -10f;
    [SerializeField] private float _maxX = 15f;

    public Rigidbody2D Rb { get; private set; }
    public BoxCollider2D Collider { get; private set; }

    public bool IsGrounded { get; private set; }
    public float JumpForce => _jumpForce;
    public Vector2 OriginalColliderSize { get; private set; }
    public Vector2 OriginalColliderOffset { get; private set; }

    private float _horizontalInput;
    private float _knockbackVelocityX;
    private float _knockbackTimeReminging;
    private bool _isCrouching;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();

        OriginalColliderSize = Collider.size;
        OriginalColliderOffset = Collider.offset;
    }

     private void FixedUpdate()
    {
        float horizontalVelocity;

        if(_knockbackTimeReminging > 0f)
        {
            // Пока действует отбрасывание, ввод игрока не влияет на X-скорость.
            horizontalVelocity = _knockbackVelocityX;
            _knockbackTimeReminging -= Time.fixedDeltaTime;
        }
        else
        {
            _knockbackTimeReminging = 0f;
            _knockbackVelocityX = 0f;
            
            float speed = _isCrouching
                ? _moveSpeed * _crouchSpeedMultiplier
                : _moveSpeed;

            horizontalVelocity = _horizontalInput * speed;
        }

        Rb.linearVelocity = new Vector2(horizontalVelocity, Rb.linearVelocity.y);

        if(_useLevelBounds)
        {
            Vector2 position = Rb.position;
            position.x = Mathf.Clamp(position.x, _minX, _maxX);
            Rb.position = position;
        }

    }

    private void Update()
    {
        IsGrounded = _groundCheck != null &&
            Physics2D.OverlapCircle(
                _groundCheck.position,
                _groundCheckRadius,
                _groundLayer
            );
    }

    public void SetHorizontalInput(float input)
    {
        _horizontalInput = Mathf.Clamp(input, -1f, 1f);
    }    public void SetCrouching(bool value)
    {
        _isCrouching = value;

        if (value)
        {
            Collider.size = new Vector2(
                OriginalColliderSize.x,
                OriginalColliderSize.y / 2f
            );

            Collider.offset = new Vector2(
                OriginalColliderOffset.x,
                OriginalColliderOffset.y - OriginalColliderSize.y / 4f
            );
        }
        else
        {
            Collider.size = OriginalColliderSize;
            Collider.offset = OriginalColliderOffset;
        }
    }

    public void Jump()
    {
        if (!IsGrounded)
            return;

        Rb.linearVelocity = new Vector2(
            Rb.linearVelocity.x,
            _jumpForce
        );
    }

    /// <summary>
    /// Отталкивает игрока в сторону от источника попадания.
    /// </summary>
    public void ApplyKnockback(
        Vector2 sourcePosition,
        float speed,
        float duraction)
    {
        if (speed <= 0f || duraction <= 0f)
            return;

        float diffirenceX = Rb.position.x - sourcePosition.x;

        // Враг слева — игрок летит вправо.
        // Враг справа — игрок летит влево.
        float direction = Mathf.Abs (diffirenceX) > 0.01f 
            ? Mathf.Sign(diffirenceX) 
            : transform.localScale.x >= 0f ? -1f : 1f;

        _horizontalInput = 0f;
        _knockbackVelocityX = direction * speed;
        _knockbackTimeReminging = duraction;
    }

   
}
