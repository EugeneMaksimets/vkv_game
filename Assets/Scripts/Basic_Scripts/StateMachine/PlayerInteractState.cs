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
            if(_playerStateController.Interectable.CurrentTag == "pick_up_item")
            {
                Debug.Log("Try pick up");
                _playerStateController.Animator.SetTrigger("PickUpItem");
            }
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