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
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Animator animator;
    [SerializeField] Animator moneyBagAnimator;
    [SerializeField] Animator batonAnimator;
    [SerializeField] Animator[] weaponsAnimators;
    [SerializeField] GameObject gunsParent;

    [Header("Values")]
    [SerializeField] float speedIdleThreshold = 0.2f;
    [SerializeField] float layerWeightChangeTime = 0.1f;

    LocomotionState curState = LocomotionState.IDLE;

    LocomotionState lastState = LocomotionState.IDLE;

    int curWeaponIndex = 0;

    public AudioSource walkSfx;

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

        //animator.SetFloat("aimAngle", controller.AimAngleClamped);

        animator.SetBool("isCarrying", playerBag.IsCarrying);

        animator.SetBool("isReloading", playerShoot.IsReloading);

        weaponsAnimators[curWeaponIndex].SetBool("isReloading", playerShoot.IsReloading);
        //animator.SetFloat("walkingSpeed", controller.RunningBackward ? -1f : 1f);
    }

    void ProcessCurState()
    {
        if (playerHealth.IsGettingUp)
        {
            curState = LocomotionState.REZ;
            return;
        }
        else if (playerHealth.IsNotAlive)
        {
            curState = LocomotionState.DEAD;
            return;
        }

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
            walkSfx.enabled = false;
            return;
        }
        else
        {
            curState = LocomotionState.WALK;
            walkSfx.enabled = true;
            return;
        }
    }

    void ProcessLastState()
    {
        if (lastState != LocomotionState.MONEY_BAG && curState == LocomotionState.MONEY_BAG)
        {
            moneyBagAnimator.SetTrigger("collect");
            //print("aaa");
        }

        if (lastState != LocomotionState.DEAD && lastState != LocomotionState.REZ &&
        (curState == LocomotionState.DEAD || curState == LocomotionState.REZ))
        {
            SetAnimatorWeight(animator, 1, false, layerWeightChangeTime);
            SetAnimatorWeight(animator, 2, false, layerWeightChangeTime);
            //animator.SetLayerWeight(1, 0f);
            //animator.SetLayerWeight(2, 0f);
        }
        else if ((lastState == LocomotionState.DEAD || lastState == LocomotionState.REZ) && curState != LocomotionState.DEAD && curState != LocomotionState.REZ)
        {
            //animator.SetLayerWeight(1, 1f);
            //animator.SetLayerWeight(2, 1f);
            SetAnimatorWeight(animator, 1, true, layerWeightChangeTime);
            SetAnimatorWeight(animator, 2, true, layerWeightChangeTime);
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

        if (playerHealth.IsNotAlive) showGun = false;

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

    void SetAnimatorWeight(Animator _animator, int _layerIndex, bool _increase, float _time)
    {
        StartCoroutine(SetAnimatorLayerWeightCoroutine(_animator, _layerIndex, _increase, _time));
    }

    IEnumerator SetAnimatorLayerWeightCoroutine(Animator _animator, int _layerIndex, bool _increase, float _time)
    {
        float t = 0f;

        float a = _increase ? 0f : 1f;
        float b = _increase ? 1f : 0f;

        while (t < _time)
        {
            _animator.SetLayerWeight(_layerIndex, Mathf.Lerp(a, b, t / _time));

            t += Time.deltaTime;
            yield return null;
        }

        _animator.SetLayerWeight(_layerIndex, b);
    }
}
