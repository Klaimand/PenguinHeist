using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    public void Interact(PlayerInteraction _playerInteraction);

    public float GetInteractionDuration();

    public bool IsInteractable();
}
