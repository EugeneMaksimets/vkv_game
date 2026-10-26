using UnityEngine;

/// <summary>
/// Перемещает врага по горизонтали и разворачивает его в сторону движения.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;

    public float MoveSpeed => _moveSpeed;
    public bool IsMoving => Mathf.Abs(_direction) > 0.01f;

    private Rigidbody2D _rigidbody;
    private float _direction;
    private Vector3 _originalScale;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _originalScale = transform.localScale;
    }

    public void MoveTowards(Transform target)
    {
        if (target == null)
        {
            Stop();
            return;
        }

        SetDirection(Mathf.Sign(target.position.x - transform.position.x));
    }

    public void SetDirection(float direction)
    {
        _direction = Mathf.Clamp(direction, -1f, 1f);

        if (Mathf.Abs(_direction) > 0.01f)
            UpdateFacingDirection();
    }

    public void Stop()
    {
        _direction = 0f;
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector2(
            _direction * _moveSpeed,
            _rigidbody.linearVelocity.y
        );
    }

    private void UpdateFacingDirection()
    {
        float scaleX = Mathf.Abs(_originalScale.x) * Mathf.Sign(_direction);
        transform.localScale = new Vector3(scaleX, _originalScale.y, _originalScale.z);
    }

    private void OnValidate()
    {
        _moveSpeed = Mathf.Max(0f, _moveSpeed);
    }
}
