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
        INTERACTING = 5
    }


    [Header("References")]
    [SerializeField] PlayerController2 controller;
    [SerializeField] PlayerShoot playerShoot;
    [SerializeField] Animator animator;

    [Header("Values")]
    [SerializeField] float speedIdleThreshold = 0.2f;

    LocomotionState curState = LocomotionState.IDLE;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (controller.Speed < speedIdleThreshold)
        {
            curState = LocomotionState.IDLE;
        }
        else
        {
            curState = LocomotionState.WALK;
        }

        animator.SetInteger("playerState", (int)curState);

        animator.SetFloat("aimAngle", controller.AimAngleClamped);
    }
}
