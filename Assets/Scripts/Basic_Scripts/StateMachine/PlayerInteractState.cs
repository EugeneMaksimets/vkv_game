using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractState : IPlayerState
{
    private PlayerStateController _playerStateController;

    public PlayerInteractState(PlayerStateController playerStateController)
    {
        _playerStateController = playerStateController;
    }
    
    public void Enter()
    {
        // _playerStateController.StopMoving();

        // Проверяем, есть ли перед нами интерактивный объект
        if (_playerStateController.Interectable != null && 
            _playerStateController.Interectable.CurrentInteractable != null)
        {
            _playerStateController.Interectable.CurrentInteractable.Interact();
        }

        //TODO: change logic with animation
            _playerStateController.StateMachine.ChangeState(_playerStateController.IdleState);
            
    }

    public void UpdateState() 
    { 
    }

    public void Exit()
    {

    }
}