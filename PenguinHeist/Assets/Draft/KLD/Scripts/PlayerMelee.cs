using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{

    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerBag playerBag;
    [SerializeField] PlayerAnimationController animationController;

    [SerializeField] float meleeAttackDuration = 0.49f;

    bool isAttacking = false;
    public bool IsAttacking => isAttacking;

    void Update()
    {
        CheckMelee();
    }

    void CheckMelee()
    {
        if (playerInteraction.IsInteracting) return;

        if (playerBag.IsCarrying) return;

        //if player is dead return

        if (isAttacking) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            animationController.Melee();
            isAttacking = true;
            StartCoroutine(WaitAndCanAttack());
        }
    }

    IEnumerator WaitAndCanAttack()
    {
        yield return new WaitForSeconds(meleeAttackDuration);
        isAttacking = false;
    }
}
