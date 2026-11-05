using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerBrain))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerBrain _playerBrain;

    void Awake()
    {
        _playerBrain = GetComponent<PlayerBrain>();
    }

    private void Update()
    {
        if(_playerBrain == null)
            return;

        // Не считываем новые команды, пока состояние урона или смерти блокирует управление.
        if (_playerBrain.IsControlLocked)
        {
            _playerBrain.StopMoving();
            return;
        }

        //Check Joystick
        if(_playerBrain.IsJoystickActive)
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

        _playerBrain.SetHorizontalInput(horizontal);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            _playerBrain.Jump();

        if (Keyboard.current.fKey.wasPressedThisFrame)
            _playerBrain.Attack();

        bool crouch =
            Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed;

        if (crouch)
            _playerBrain.Crouch();
        else if (
            Keyboard.current.sKey.wasReleasedThisFrame ||
            Keyboard.current.downArrowKey.wasReleasedThisFrame)
            _playerBrain.StandUp();

        bool isInteractPressed = 
            Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
        if(isInteractPressed)
            _playerBrain.Interact();
    }
}
