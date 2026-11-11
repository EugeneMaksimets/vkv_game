public class PlayerInteractState : IPlayerState
{
    private readonly PlayerInteraction _playerInteraction;
    private readonly PlayerAnimation _playerAnimation;
    private const string TagPickUpItem = "pick_up_item";

    public PlayerInteractState(PlayerInteraction interaction, PlayerAnimation animation)
    {
        _playerInteraction = interaction;
        _playerAnimation = animation;
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
    }

    public void UpdateState() { }

    public void Exit() { }
}