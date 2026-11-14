using System;
using UnityEngine;

public class PlayerInteractState : IPlayerState
{
    private const string TagPickUpItem = "pick_up_item";
    private readonly PlayerInteraction _playerInteraction;
    private readonly PlayerAnimation _playerAnimation;
    private readonly float _fallbackTimeout;
    private float _timer;
    private bool _isCompleted;
    private event Action InteractCompleted;

    public PlayerInteractState(
        PlayerInteraction interaction, 
        PlayerAnimation animation, 
        float fallbackTimeout = 1.5f)
    {
        _playerInteraction = interaction;
        _playerAnimation = animation;
        _fallbackTimeout = Mathf.Max(0.1f, fallbackTimeout);
    }

    public void Enter()
    {
        _timer = 0f;
        _isCompleted = false;

        var interactable = _playerInteraction.CurrentInteractable;

        if (interactable == null)
        {
            Complete();
            return;
        }

        bool hasPickUpAnimation = _playerInteraction.CurrentTag == TagPickUpItem;
        if (hasPickUpAnimation)
        {
            _playerAnimation.InteractionAnimationFinished += Complete;
            _playerAnimation.PlayPickUpItem();
        }

        interactable.Interact();

        // if animation dont work comlete state
        if (!hasPickUpAnimation)
        {
            Complete();
        }
    }

    public void UpdateState()
    {
        if (_isCompleted)
            return;

        _timer += Time.deltaTime;
        if (_timer >= _fallbackTimeout)
        {
            Debug.LogWarning($"{nameof(PlayerInteractState)}: Interaction animation timed out.");
            Complete();
        }
    }

    public void Exit()
    {
        _playerAnimation.InteractionAnimationFinished -= Complete;
    }

    private void Complete()
    {
        if (_isCompleted)
            return;

        _isCompleted = true;
        InteractCompleted?.Invoke();
    }
}