using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerStateController _player;

    private void Update()
    {
        if(_player == null)
            return;

        //Check Joystick
        if(_player.IsJoystickActive)
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

        _player.SetHorizontalInput(horizontal);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            _player.Jump();

        if (Keyboard.current.fKey.wasPressedThisFrame)
            _player.Attack();

        bool crouch =
            Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed;

        if (crouch)
            _player.Crouch();
        else if (
            Keyboard.current.sKey.wasReleasedThisFrame ||
            Keyboard.current.downArrowKey.wasReleasedThisFrame)
            _player.StandUp();
    }
}