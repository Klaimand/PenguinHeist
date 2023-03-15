using System;
using UnityEngine;

public class CustomizationAnimationManager : MonoBehaviour
{
    public static CustomizationAnimationManager instance;

    [SerializeField] private Animator player1Lever;
    [SerializeField] private Animator player2Lever;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerLever(int playerIndex)
    {
        if (playerIndex == 0)
        {
            player1Lever.SetTrigger("confirm");
        }
        else
        {
            player2Lever.SetTrigger("confirm");
        }
    }

    public void TriggerItemButton(Animator animator)
    {
        animator.SetTrigger("nextItem");
    }
}
