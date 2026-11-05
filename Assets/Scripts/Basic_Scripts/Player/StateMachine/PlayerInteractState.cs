using UnityEngine;

public class PlayerInteractState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;

    public PlayerInteractState(PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
    }

    public void Enter()
    {
        var interactable = _playerBrain.Interectable;

        if (interactable != null &&
            interactable.CurrentInteractable != null)
        {
            if (interactable.CurrentTag == "pick_up_item")
            {
                Debug.Log("Try pick up");

                _playerBrain.Animator.SetTrigger("PickUpItem");
            }

            interactable.CurrentInteractable.Interact();
        }

        // TODO: Пересмотреть логику перехода после анимации.
        _playerBrain.StateMachine.ChangeState(
            _playerBrain.IdleState
        );
    }

    public void UpdateState()
    {
    }

    public void Exit()
    {
    }
}