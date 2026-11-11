using System;

public class PlayerInteractState : IPlayerState
{
    private readonly PlayerBrain _playerBrain;
    private readonly PlayerInteraction _playerInteraction;
    private readonly PlayerAnimation _playerAnimation;
    private const string TagPickUpItem = "pick_up_item";

    public PlayerInteractState(
        PlayerBrain playerBrain,
        PlayerInteraction playerInteraction,
        PlayerAnimation playerAnimation)
    {
        _playerBrain = playerBrain;
        _playerInteraction = playerInteraction;
        _playerAnimation = playerAnimation;
    }

    public void Enter()
    {
        var interactable = _playerInteraction.CurrentInteractable;

        if (interactable != null)
        {
            if (_playerInteraction.CurrentTag == TagPickUpItem)
            {
                _playerAnimation.PlayPickUpItem();
            }

            interactable.Interact();
        }

        // TODO: Пересмотреть переход после анимации.
        _playerBrain.StateMachine.ChangeState(
            _playerBrain.IdleState
        );
    }

    public void UpdateState() { }

    public void Exit() { }
}