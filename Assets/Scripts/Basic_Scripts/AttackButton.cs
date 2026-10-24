using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler {

    [SerializeField] private PlayerStateController _playerStateController;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_playerStateController != null)
        {
            _playerStateController.Attack();
        }
    }
    
}