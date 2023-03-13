using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public enum LocomotionState
    {
        IDLE = 1,
        WALK = 2,
        DEAD = 3,
        REZ = 4,
        INTERACTING = 5,
        MONEY_BAG = 7
    }


    [Header("References")]
    [SerializeField] PlayerController2 controller;
    [SerializeField] PlayerShoot playerShoot;
    [SerializeField] PlayerBag playerBag;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] Animator animator;
    [SerializeField] Animator moneyBagAnimator;
    [SerializeField] Animator batonAnimator;
    [SerializeField] Animator[] weaponsAnimators;

    [Header("Values")]
    [SerializeField] float speedIdleThreshold = 0.2f;

    LocomotionState curState = LocomotionState.IDLE;

    LocomotionState lastState = LocomotionState.IDLE;

    void OnEnable()
    {
        playerShoot.OnPlayerShoot += Shoot;
    }

    void OnDisable()
    {
        playerShoot.OnPlayerShoot -= Shoot;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProcessCurState();

        ProcessLastState();

        animator.SetInteger("playerState", (int)curState);

        animator.SetFloat("aimAngle", controller.AimAngleClamped);

        animator.SetBool("isCarrying", playerBag.IsCarrying);
        //animator.SetFloat("walkingSpeed", controller.RunningBackward ? -1f : 1f);
    }

    void ProcessCurState()
    {


        if (playerInteraction.IsInteracting)
        {
            if (playerBag.IsCarrying)
                curState = LocomotionState.MONEY_BAG;
            else
                curState = LocomotionState.INTERACTING;

            return;
        }

        if (controller.Speed < speedIdleThreshold)
        {
            curState = LocomotionState.IDLE;
        }
        else
        {
            curState = LocomotionState.WALK;
        }
    }

    void ProcessLastState()
    {
        if (lastState != LocomotionState.MONEY_BAG && curState == LocomotionState.MONEY_BAG)
        {
            moneyBagAnimator.SetTrigger("collect");
            //print("aaa");
        }

        //if (lastState == LocomotionState.MONEY_BAG && curState != LocomotionState.MONEY_BAG)
        //{
        //    moneyBagAnimator.SetTrigger("drop");
        //}

        lastState = curState;
    }

    void Shoot()
    {
        animator.SetTrigger("shoot");

        foreach (var curAnimator in weaponsAnimators)
        {
            curAnimator.SetTrigger("shoot");
        }
    }

    public void LaunchBag()
    {
        moneyBagAnimator.SetTrigger("drop");
    }

    public void Melee()
    {
        animator.SetTrigger("melee");
        batonAnimator.SetTrigger("hit");
    }
}
