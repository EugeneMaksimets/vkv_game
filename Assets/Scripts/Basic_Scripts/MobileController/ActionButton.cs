using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerDownHandler {
    [SerializeField] private PlayerStateController _playerStateController;
    public void OnPointerDown(PointerEventData eventData) 
    {
        
    }
}
