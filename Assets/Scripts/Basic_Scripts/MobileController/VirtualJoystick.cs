using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private float _deadZone = 25f;

    [Header("Спрайты джойстика (Сетка 3х3)")]
    // Индексы спрайтов в массиве
    // 0 - Центр, 1 - Клик, 2 - Вправо, 3 - Влево, 4 - Верх, 5 - Вниз 
    [SerializeField] private Sprite[] _joystickSprites; 

    private PlayerBrain _playerBrain;
    private Image _joystickImage;
    private RectTransform _rectTransform;
    private Vector2 _inputVector = Vector2.zero;
    private bool _isPointerHeld;
    
    private void Awake()
    {
        _joystickImage = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerHeld = true;

        if (_playerBrain != null)
            _playerBrain.IsJoystickActive = true;

        HandleJoystick(eventData.position, eventData.pressEventCamera);
    }

    public void OnDrag(PointerEventData eventData)
    {
        HandleJoystick(eventData.position, eventData.pressEventCamera);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerHeld = false;
        _inputVector = Vector2.zero;

        if (_playerBrain != null)
        {
            _playerBrain.SetHorizontalInput(0f);
            _playerBrain.StandUp();
            _playerBrain.IsJoystickActive = false;
        }

        if (_joystickSprites != null &&
            _joystickSprites.Length > 0 &&
            _joystickSprites[0] != null)
        {
            _joystickImage.sprite = _joystickSprites[0];
        }
        
    }

    public void Initialize(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;

        if (_playerBrain == null)
        {
            Debug.LogError("[VirtualJoystick] PlayerBrain is null!");
            return;
        }
    }

    private void HandleJoystick(Vector2 screenPosition, Camera cam)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, screenPosition, cam, out localPoint))
        {
            float x = localPoint.x;
            float y = localPoint.y;

            // Сбрасываем вектор ввода перед пересчетом
            _inputVector = Vector2.zero;

            // --- 1. ГОРИЗОНТАЛЬНОЕ ДВИЖЕНИЕ (Влево / Вправо) ---
            if (x < -_deadZone)
            {
                _inputVector.x = -1f;

                // Спрайт влево (индекс 3)

                _joystickImage.sprite = _joystickSprites[3];
            }
            else if (x > _deadZone)
            {
                _inputVector.x = 1f;
                // Спрайт вправо (индекс 2)
                if (_joystickSprites.Length > 2 && _joystickSprites[2] != null) 
                    _joystickImage.sprite = _joystickSprites[2];
            }

            // --- 2. ВЕРТИКАЛЬНОЕ ДВИЖЕНИЕ (Прыжок / Приседание) ---
            if (y > _deadZone)
            {
                // Палец потянули вверх -> Прыжок
                if (_playerBrain != null) _playerBrain.Jump();
                
                // Спрайт верха (индекс 4)
                if (_joystickSprites.Length > 4 && _joystickSprites[4] != null) 
                    _joystickImage.sprite = _joystickSprites[4];
            }
            else if (y < -_deadZone)
            {
                // Палец потянули вниз -> Приседание
                if (_playerBrain != null) _playerBrain.Crouch();
                
                // Спрайт низа (индекс 5)
                if (_joystickSprites.Length > 5 && _joystickSprites[5] != null) 
                    _joystickImage.sprite = _joystickSprites[5];
            }
            else
            {
                // Если по вертикали палец в нейтральной зоне — встаем из приседа
                if (_playerBrain != null) _playerBrain.StandUp();
            }

            // --- 3. ЕСЛИ ВСЁ В МЕРТВОЙ ЗОНЕ — ЦЕНТР ---
            if (Mathf.Abs(x) < _deadZone && Mathf.Abs(y) < _deadZone)
            {
                if (_joystickSprites.Length > 0 && _joystickSprites[0] != null)
                    _joystickImage.sprite = _joystickSprites[0];
                
                if (_playerBrain != null) 
                    _playerBrain.StandUp();
            }

            ApplyInput();
        }
    }

    private void ApplyInput()
    {
        if (_playerBrain == null)
            return;

        if (!_isPointerHeld)
            return;

        // Джойстик активен, пока палец удерживается,
        // даже если он находится в dead zone.
        _playerBrain.IsJoystickActive = true;

        _playerBrain.SetHorizontalInput(_inputVector.x);
    }

}