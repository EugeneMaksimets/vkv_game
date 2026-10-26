using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler {

    [SerializeField] private PlayerStateController _playerStateController;

    private void Awake()
    {
        // Если забыл перетащить в инспекторе, находим игрока автоматически на сцене
        if (_playerStateController == null)
        {
            _playerStateController = FindAnyObjectByType<PlayerStateController>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (_playerStateController != null)
        {
            _playerStateController.Attack();
        }
        else
        {
            Debug.LogError("PlayerStateController не найден на сцене!");
        }
    }
}