using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerStateController))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerStateController _playerStateController;

    void Awake()
    {
        _playerStateController = GetComponent<PlayerStateController>();
    }

    private void Update()
    {
        if(_playerStateController == null)
            return;

        // Не считываем новые команды, пока состояние урона или смерти блокирует управление.
        if (_playerStateController.IsControlLocked)
        {
            _playerStateController.StopMoving();
            return;
        }

        //Check Joystick
        if(_playerStateController.IsJoystickActive)
            return;

        if (Keyboard.current == null)
            return;

        float horizontal = 0f;

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
            horizontal = -1f;

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
            horizontal = 1f;

        _playerStateController.SetHorizontalInput(horizontal);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            _playerStateController.Jump();

        if (Keyboard.current.fKey.wasPressedThisFrame)
            _playerStateController.Attack();

        bool crouch =
            Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed;

        if (crouch)
            _playerStateController.Crouch();
        else if (
            Keyboard.current.sKey.wasReleasedThisFrame ||
            Keyboard.current.downArrowKey.wasReleasedThisFrame)
            _playerStateController.StandUp();

        bool isInteractPressed = 
            Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
        if(isInteractPressed)
            _playerStateController.Interact();
    }
}
