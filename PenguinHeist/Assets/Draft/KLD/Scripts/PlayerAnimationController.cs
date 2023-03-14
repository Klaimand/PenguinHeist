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
    [SerializeField] PlayerMelee playerMelee;
    [SerializeField] Animator animator;
    [SerializeField] Animator moneyBagAnimator;
    [SerializeField] Animator batonAnimator;
    [SerializeField] Animator[] weaponsAnimators;
    [SerializeField] GameObject gunsParent;

    [Header("Values")]
    [SerializeField] float speedIdleThreshold = 0.2f;

    LocomotionState curState = LocomotionState.IDLE;

    LocomotionState lastState = LocomotionState.IDLE;

    int curWeaponIndex = 0;

    void OnEnable()
    {
        playerShoot.OnPlayerShoot += Shoot;
        playerShoot.OnPlayerChangeWeapon += InitWeaponAnimator;
    }

    void OnDisable()
    {
        playerShoot.OnPlayerShoot -= Shoot;
        playerShoot.OnPlayerChangeWeapon -= InitWeaponAnimator;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProcessGunParent();

        ProcessCurState();

        ProcessLastState();

        animator.SetInteger("playerState", (int)curState);

        animator.SetFloat("aimAngle", controller.AimAngleClamped);

        animator.SetBool("isCarrying", playerBag.IsCarrying);

        animator.SetBool("isReloading", playerShoot.IsReloading);

        weaponsAnimators[curWeaponIndex].SetBool("isReloading", playerShoot.IsReloading);
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

    void ProcessGunParent()
    {
        bool showGun = true;

        if (playerBag.IsCarrying) showGun = false;

        if (playerMelee.IsAttacking) showGun = false;

        if (playerInteraction.IsInteracting) showGun = false;

        //if is dead

        if (showGun != gunsParent.activeInHierarchy)
        {
            gunsParent.SetActive(showGun);
        }
    }

    void Shoot()
    {
        animator.SetTrigger("shoot");

        weaponsAnimators[curWeaponIndex].SetTrigger("shoot");
    }

    void InitWeaponAnimator()
    {
        weaponsAnimators[curWeaponIndex].SetBool("isReloading", false);

        for (int i = 0; i < weaponsAnimators.Length; i++)
        {
            if (i == playerShoot.CurWeapon.weaponIndex)
            {
                curWeaponIndex = i;
                weaponsAnimators[i].transform.parent.gameObject.SetActive(true);
            }
            else
            {
                weaponsAnimators[i].transform.parent.gameObject.SetActive(false);
            }
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
