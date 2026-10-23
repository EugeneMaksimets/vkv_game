using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private UnityEngine.UI.Image _joystickImage;
    private RectTransform _rectTransform;
    private Vector2 _inputVector = Vector2.zero;
    [SerializeField] private PlayerMovement _playerMovement; 
    [SerializeField] private float _deadZone = 25f;

    [Header("Спрайты джойстика (Сетка 3х3)")]
    // Индексы спрайтов в массиве
    // 0 - Центр, 1 - Клик, 2 - Вправо, 3 - Влево, 4 - Верх, 5 - Вниз 
    [SerializeField] private Sprite[] _joystickSprites; 
    
    void Start()
    {
        _joystickImage = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
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

        // Возвращаем спрайт центра (обычно он первый под индексом 0)
        if (_joystickSprites != null && _joystickSprites.Length > 0 && _joystickSprites[0] != null)
        {
            _joystickImage.sprite = _joystickSprites[0];
        }

        if (_playerMovement != null) 
        {
            _playerMovement.StandUp();
        }
    }
//    private void HandleJoystick(Vector2 screenPosition, Camera cam)
//     {
//         Vector2 localPoint;
//         if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, screenPosition, cam, out localPoint))
//         {
//             float x = localPoint.x;
//             float y = localPoint.y;

//             // Проверяем мертвую зону (если палец близко к центру)
//             if (Mathf.Abs(x) < _deadZone && Mathf.Abs(y) < _deadZone)
//             {
//                 _inputVector = Vector2.zero;
//                 if (_joystickSprites.Length > 0 && _joystickSprites[0] != null)
//                     _joystickImage.sprite = _joystickSprites[0];

//                 // Если вернулись в центр — встаем из приседа
//                 if (_playerMovement != null) _playerMovement.StandUp();
//             }
//             else
//             {
//                 // Определяем направление по горизонтали для движения
//                 if (x < -_deadZone)
//                 {
//                     _inputVector = new Vector2(-1f, 0f);
//                     // Если спрайт влево задан под индексом 3 
//                     if (_joystickSprites.Length > 3 && _joystickSprites[3] != null) 
//                         _joystickImage.sprite = _joystickSprites[3];
//                 }
//                 else if (x > _deadZone)
//                 {
//                     _inputVector = new Vector2(1f, 0f);
//                     // Если спрайт вправо задан под индексом 4 
//                     if (_joystickSprites.Length > 4 && _joystickSprites[4] != null) 
//                         _joystickImage.sprite = _joystickSprites[4];
//                 }

//                 // Проверяем вертикаль (Прыжок вверх или Приседание вниз)
//                 if (y > _deadZone)
//                 {
//                     // Jump
//                     if (_playerMovement != null) _playerMovement.Jump();
//                     // Если спрайт вверх задан под индексом 2
//                     if (_joystickSprites.Length > 2 && _joystickSprites[2] != null) 
//                         _joystickImage.sprite = _joystickSprites[2];
//                 }
//                 else if (y < -_deadZone)
//                 {
//                     // Палец потянули вниз -> Приседание
//                     if (_playerMovement != null) _playerMovement.Crouch();
//                     // Спрайт низа (если добавлен в массив под индексом 5)
//                     if (_joystickSprites.Length > 5 && _joystickSprites[5] != null) 
//                         _joystickImage.sprite = _joystickSprites[5];
//                 }
//             }

//             ApplyInput();
//         }
//     }

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
                if (_joystickSprites.Length > 3 && _joystickSprites[3] != null) 
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
                if (_playerMovement != null) _playerMovement.Jump();
                
                // Спрайт верха (индекс 4)
                if (_joystickSprites.Length > 4 && _joystickSprites[4] != null) 
                    _joystickImage.sprite = _joystickSprites[4];
            }
            else if (y < -_deadZone)
            {
                // Палец потянули вниз -> Приседание
                if (_playerMovement != null) _playerMovement.Crouch();
                
                // Спрайт низа (индекс 5)
                if (_joystickSprites.Length > 5 && _joystickSprites[5] != null) 
                    _joystickImage.sprite = _joystickSprites[5];
            }
            else
            {
                // Если по вертикали палец в нейтральной зоне — встаем из приседа
                if (_playerMovement != null) _playerMovement.StandUp();
            }

            // --- 3. ЕСЛИ ВСЁ В МЕРТВОЙ ЗОНЕ — ЦЕНТР ---
            if (Mathf.Abs(x) < _deadZone && Mathf.Abs(y) < _deadZone)
            {
                if (_joystickSprites.Length > 0 && _joystickSprites[0] != null)
                    _joystickImage.sprite = _joystickSprites[0];
                
                if (_playerMovement != null) 
                    _playerMovement.StandUp();
            }

            ApplyInput();
        }
    }

    private void ApplyInput()
    {
        if (_playerMovement == null) return;

        if (_inputVector.x > 0)
        {
            _playerMovement.IsJoystickActive = true;
            _playerMovement.SetMoveRight();
        }
        else if (_inputVector.x < 0)
        {
            _playerMovement.IsJoystickActive = true;
            _playerMovement.SetMoveLeft();
        }
        else
        {   
            _playerMovement.IsJoystickActive = false;
            _playerMovement.StopMoving();
        }
    }
}