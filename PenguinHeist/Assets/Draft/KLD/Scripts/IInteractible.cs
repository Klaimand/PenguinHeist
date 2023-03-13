using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    DEFAULT,
    MONEY_BAG
}

public interface IInteractible
{
    public void InteractImmediate(PlayerInteraction _playerInteraction);

    public void Interact(PlayerInteraction _playerInteraction);

    public float GetInteractionDuration();

    public bool IsInteractable();

    public InteractionType GetInteractionType();
}
