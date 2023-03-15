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
<<<<<<< Updated upstream
=======
    
    [Header("Input")]
    public string meleeInput;
    public float lTrigger;
    public bool isPressingMeleeInput;

    public AudioSource meleeSfx;

    private void Start()
    {
        var playerIndex = GetComponent<PlayerController2>().playerIndex;
        meleeInput = playerIndex switch
        {
            0 => $"LeftTrigger",
            1 => $"LeftTriggerP2",
            _ => meleeInput
        };
    }
>>>>>>> Stashed changes

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
            meleeSfx.Play();
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
