using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBag : MonoBehaviour, IInteractible
{
    [SerializeField] float interactionDuration = 1f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetInteractionDuration()
    {
        return interactionDuration;
    }

    public void Interact(PlayerInteraction _playerInteraction)
    {
        gameObject.SetActive(false);
    }

    public bool IsInteractable()
    {
        return true;
    }
}
