using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBag : MonoBehaviour, IInteractible
{
    //[SerializeField] float interactionDuration = 1f;

    public float GetInteractionDuration()
    {
        return 0.6f;
    }

    public void Interact(PlayerInteraction _playerInteraction)
    {
    }

    public void InteractImmediate(PlayerInteraction _playerInteraction)
    {
        _playerInteraction.PplayerBag.CarryBag(this);
        Destroy(gameObject);
    }

    public bool IsInteractable()
    {
        return true;
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.MONEY_BAG;
    }

}
