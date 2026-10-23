
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

    private float _horizontalInput;
    private bool _isCrouching;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
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

    private void FixedUpdate()
    {
        float speed = _isCrouching
            ? _moveSpeed * _crouchSpeedMultiplier
            : _moveSpeed;

        Rb.linearVelocity = new Vector2(
            _horizontalInput * speed,
            Rb.linearVelocity.y
        );

        if (_useLevelBounds)
        {
            Vector2 position = Rb.position;
            position.x = Mathf.Clamp(position.x, _minX, _maxX);
            Rb.position = position;
        }
    }

        public void SetHorizontalInput(float input)
    {
        _horizontalInput = Mathf.Clamp(input, -1f, 1f);
    }

    public void SetCrouching(bool value)
    {
        _isCrouching = value;
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

}
