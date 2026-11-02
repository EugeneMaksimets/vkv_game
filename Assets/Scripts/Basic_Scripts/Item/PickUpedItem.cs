using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PickUpedItem : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform _playerItemHolder;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private int _sortingLayer = 10;
    [SerializeField] private float _returnDelay = 1f;

    [Header("Scale Settings")]
    [SerializeField] private Vector3 _targetScale = Vector3.one * 1f;
    [SerializeField] private Vector3 _finalScale = Vector3.one * 0.1f;
    [SerializeField] private float _scaleSpeed = 5f;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private ItemState _currentState = ItemState.Idle;
    private float _waitTimer;
    private bool _isCloseEnoughToTarget = false;
    private bool _isScaleChanged = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;

        }

        if (_playerItemHolder == null)
        {
            GameObject itemHolder = GameObject.FindGameObjectWithTag("PlayerItemHolder");
            if (itemHolder != null)
            {
                _playerItemHolder = itemHolder.transform;
            }
            else
            {
                Debug.LogWarning("PlayerItemHolder not found in the scene. Please assign the PlayerItemHolder Transform manually.");
            }
        }
    }

    private void FixedUpdate()
    {
        switch (_currentState)
        {
            case ItemState.MovingToCamera:
                ChangePosition(_mainCamera.transform.position); // Change the scale to the target scale when moving to the camera center
                _isCloseEnoughToTarget = IsCloseEnoughToTarget(_mainCamera.transform.position); 
                if(_isCloseEnoughToTarget && _isScaleChanged)
                {
                    ChangeState(ItemState.Waiting);
                }
                break;

            case ItemState.ReturningToPlayer:
                ChangePosition(_playerItemHolder.position); // Return to the player item holder position
                _isCloseEnoughToTarget = IsCloseEnoughToTarget(_playerItemHolder.position); 
                if(_isCloseEnoughToTarget && _isScaleChanged)
                {
                    ChangeState(ItemState.Destroy);
                }
                break;

            case ItemState.Waiting:
                WaitBeforeReturn();
                break;
            case ItemState.Idle:
                break;
            case ItemState.Deactivated:
                gameObject.SetActive(false); // Deactivate the item after it reaches the player
                break;
            case ItemState.Destroy:
                Destroy(gameObject); // Destroy the item after it reaches the player
                break;            
            default:
                break;
        }
        
    }

    private void Update()
    {
        switch (_currentState)
        {
            case ItemState.MovingToCamera:
                ChangeScale(_targetScale); // Change the scale to the target scale when moving to the camera center
                _isScaleChanged = IsScaleChanged(_targetScale);
                break;

            case ItemState.ReturningToPlayer:
                ChangeScale(_finalScale); // Change the scale to the final scale while returning to the player
                _isScaleChanged = IsScaleChanged(_finalScale);
                break;

            default:
                break;
        }
    }

    public void Interact()
    {
        if (_mainCamera == null || _playerItemHolder == null)
        {
            Debug.LogError("PickUpedItem: References are missing.", this);
            return;
        }

        if (_currentState != ItemState.Idle)
        {
            return;
        }

        _currentState = ItemState.MovingToCamera;
        _spriteRenderer.sortingOrder = _sortingLayer;
    }

    private void ChangeState(ItemState state)
    {
       
        if (_currentState != state && _isCloseEnoughToTarget && _isScaleChanged) // Only change state if the item is close enough to the target position
        {
            _currentState = state;
            _waitTimer = 0f;
        }

        if (state == ItemState.MovingToCamera ||
            state == ItemState.ReturningToPlayer)
        {
            _isCloseEnoughToTarget = false;
            _isScaleChanged = false;
        }
    }

    private bool IsCloseEnoughToTarget(Vector3 targetPosition, float threshold = 0.05f)
    {
        return Vector2.Distance(_rb.position, targetPosition) < threshold;
    }

    private bool IsScaleChanged(Vector3 targetScale, float threshold = 0.01f)
    {
        return Vector3.Distance(transform.localScale, targetScale) < threshold;
    }

    private void ChangePosition(Vector3 targetPosition)
    {
        
        targetPosition.z = transform.position.z; // Keep the item's z position unchanged
        Vector3 newPosition = Vector3.MoveTowards(
            transform.position, 
            targetPosition, 
            _moveSpeed * Time.fixedDeltaTime
        );

        _rb.MovePosition(newPosition);

    }
    


    private void ChangeScale(Vector3 newScale)
    {
        transform.localScale = Vector3.MoveTowards(
            transform.localScale,
            newScale,
            _scaleSpeed * Time.deltaTime
        );
    }

    private void WaitBeforeReturn()
    {
        _waitTimer += Time.fixedDeltaTime;

        if (_waitTimer >= _returnDelay)
        {
            ChangeState(ItemState.ReturningToPlayer);
        }
    }

    
}
