using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// Подключаем пространство имен нового Input System
using UnityEngine.InputSystem;

public class UIClickRaycastTest : MonoBehaviour
{
    private GraphicRaycaster _raycaster;
    private PointerEventData _pointerEventData;
    private EventSystem _eventSystem;

    void Start()
    {
        _raycaster = GetComponent<GraphicRaycaster>();
        if (_raycaster == null)
        {
            _raycaster = gameObject.AddComponent<GraphicRaycaster>();
        }

        _eventSystem = EventSystem.current;
        if (_eventSystem == null)
        {
            Debug.LogError("В сцене не найден EventSystem!");
        }
    }

    void Update()
    {
        // Проверяем клик мыши через новый Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckUIElements(Mouse.current.position.ReadValue());
        }
        // На случай касания экрана на мобильном устройстве через Touchscreen
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            CheckUIElements(Touchscreen.current.primaryTouch.position.ReadValue());
        }
    }

    private void CheckUIElements(Vector2 screenPosition)
    {
        if (_eventSystem == null) return;

        _pointerEventData = new PointerEventData(_eventSystem);
        _pointerEventData.position = screenPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(_pointerEventData, results);

        Debug.Log($"--- Найдено UI-объектов под курсором: {results.Count} ---");
        foreach (RaycastResult result in results)
        {
            Debug.Log($"Врезалось в: **{result.gameObject.name}** (Компонент: {result.module.GetType().Name})", result.gameObject);
        }
    }
}