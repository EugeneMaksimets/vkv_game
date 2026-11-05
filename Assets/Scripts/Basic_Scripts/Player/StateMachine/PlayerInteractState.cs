
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
                _playerBrain.Animation.PlayPickUpItem();
            }

            interactable.CurrentInteractable.Interact();
        }

        // TODO: Пересмотреть переход после анимации.
        _playerBrain.StateMachine.ChangeState(
            _playerBrain.IdleState
        );
    }

    public void UpdateState() { }

    public void Exit() { }
}
