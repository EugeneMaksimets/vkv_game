using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerStateController))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerStateController _playerStateControler;

    void Awake()
    {
        _playerStateControler = GetComponent<PlayerStateController>();
    }

    private void Update()
    {
        if(_playerStateControler == null)
            return;

        //Check Joystick
        if(_playerStateControler.IsJoystickActive)
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

        _playerStateControler.SetHorizontalInput(horizontal);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            _playerStateControler.Jump();

        if (Keyboard.current.fKey.wasPressedThisFrame)
            _playerStateControler.Attack();

        bool crouch =
            Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed;

        if (crouch)
            _playerStateControler.Crouch();
        else if (
            Keyboard.current.sKey.wasReleasedThisFrame ||
            Keyboard.current.downArrowKey.wasReleasedThisFrame)
            _playerStateControler.StandUp();
    }
}