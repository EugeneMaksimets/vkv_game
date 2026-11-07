using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Image _joystickImage;
    private RectTransform _rectTransform;
    private Vector2 _inputVector = Vector2.zero;
    [SerializeField] private PlayerBrain _playerBrain; 
    [SerializeField] private float _deadZone = 25f;

    [Header("Спрайты джойстика (Сетка 3х3)")]
    // Индексы спрайтов в массиве
    // 0 - Центр, 1 - Клик, 2 - Вправо, 3 - Влево, 4 - Верх, 5 - Вниз 
    [SerializeField] private Sprite[] _joystickSprites; 
    
    void Awake()
    {
        // Если забыл перетащить в инспекторе, находим игрока автоматически на сцене
        if (_playerBrain == null)
        {
            _playerBrain = FindAnyObjectByType<PlayerBrain>();
        }
        
        _joystickImage = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
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
        _inputVector = Vector2.zero;
        ApplyInput();

        if (_playerBrain != null)
        {
            _playerBrain.StopMoving();
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
                if(_playerBrain != null) _playerBrain.SetMoveRight();
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

    // private void ApplyInput()
    // {
    //     if (_playerBrain == null)
    //         return;

    //     _playerBrain.SetHorizontalInput(_inputVector.x);
    // }

    private void ApplyInput()
    {
        if (_playerBrain == null) return;

        if (_inputVector.x > 0)
        {
            _playerBrain.IsJoystickActive = true;
            _playerBrain.SetMoveRight();
        }
        else if (_inputVector.x < 0)
        {
            _playerBrain.IsJoystickActive = true;
            _playerBrain.SetMoveLeft();
        }
        else
        {   
            _playerBrain.IsJoystickActive = false;
            _playerBrain.StopMoving();
        }
    }
}