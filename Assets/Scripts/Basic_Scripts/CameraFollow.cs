using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target; // Ссылка на Курана
    [SerializeField] private float _smoothSpeed = 0.125f;

    [Header("Позиционирование камеры")]
    [SerializeField] private float _fixedY = 0f;          // Фиксированная высота камеры по Y (чтобы не прыгала вместе с героем)
    [SerializeField] private float _lookAheadX = 3f;      // Сдвиг камеры вперед по X в сторону взгляда игрока

    [Header("Границы камеры")]
    [SerializeField] private bool _useCameraBounds = true;
    [SerializeField] private float _minCameraX = 0f;      // Левый край (чтобы камера не уходила в пустоту)
    [SerializeField] private float _maxCameraX = 6f;     // Правый край уровня

    void Start()
    {
        if (_target == null)
        {
            FindPlayer();
        }
    }

    void LateUpdate()
    {
        if (_target == null) {
            FindPlayer();
            return;
        }

        // check the direction of the target's scale to determine which way the player is facing
        float directionX = Mathf.Sign(_target.localScale.x);

        // Целевая позиция X: позиция игрока + упреждение вперед
        float targetX = _target.position.x + (_lookAheadX * directionX);

        // Если включены границы — жестко удерживаем камеру по горизонтали
        if (_useCameraBounds)
        {
            targetX = Mathf.Clamp(targetX, _minCameraX, _maxCameraX);
        }

        // Желаемая позиция камеры (по Y используем фиксированную высоту, чтобы не задирать в небо)
        Vector3 desiredPosition = new Vector3(targetX, _fixedY, -10f);

        // Плавное перемещение камеры
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);

        // Применяем финальную позицию
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10f);
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _target = player.transform;
        }
    }
}