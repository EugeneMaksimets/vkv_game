using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler {

    [SerializeField] private PlayerBrain _playerBrain;

    private void Awake()
    {
        // Если забыл перетащить в инспекторе, находим игрока автоматически на сцене
        if (_playerBrain == null)
        {
            _playerBrain = FindAnyObjectByType<PlayerBrain>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (_playerBrain != null)
        {
            _playerBrain.Attack();
        }
        else
        {
            Debug.LogError("PlayerStateController не найден на сцене!");
        }
    }
}