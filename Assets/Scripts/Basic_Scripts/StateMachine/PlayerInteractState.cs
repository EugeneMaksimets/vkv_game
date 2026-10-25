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
        else
        {
            Debug.Log("Рядом нет объектов для взаимодействия!");
            // Если взаимодействовать не с чем
            _playerStateController.StateMachine.ChangeState(_playerStateController.IdleState);
        }
    }

    public void UpdateState() 
    { 
    }

    public void Exit() 
    { 
    }
}