using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerShoot playerShoot;
    [SerializeField] Transform lookAtTransform = null;
    [SerializeField] Transform refTransform = null;

    [Header("Values")]
    [SerializeField] float axisDeadzone = 0.1f;

    [SerializeField] float accelerationTime = 0.3f;
    [SerializeField] float decelerationTime = 0.3f;
    [SerializeField] float speed = 8f;

    //axis
    Vector2 rawAxis = Vector2.zero;
    Vector2 timedAxis = Vector2.zero;
    float timedMagnitude = 0f;

    public enum PlayerState { DEFAULT, DEAD };
    PlayerState playerState = PlayerState.DEFAULT;

    //[SerializeField, Header("Animation")] Animator animator;
    //enum LocomotionState { IDLE, RUNNING, DIE, RESPAWNING, RUNNING_BACKWARD, WIN };
    //LocomotionState locomotionState = LocomotionState.IDLE;

    [SerializeField] float rbVelocityDead = 0.05f;
    [SerializeField] float forwardToAimAngle = 0f;
    [SerializeField] float absoluteForwardToAimAngle = 0f;
    Vector3 playerToLookAtTransform = Vector3.zero;
    bool runningBackward = false;

    float curAngleOffset = 0f;

    void Update()
    {
        ProcessAxis();

        if (playerState == PlayerState.DEFAULT)
        {
            rb.velocity = (refTransform.right * rawAxis.x + refTransform.forward * rawAxis.y) * speed;
        }

        //DoFeetRotation(); //useless

        //AnimateLocomotionState();
    }

    void FixedUpdate()
    {
        //DoFeetRotation();
    }

    void ProcessAxis()
    {
        rawAxis.x = Input.GetAxisRaw("Horizontal");
        rawAxis.y = Input.GetAxisRaw("Vertical");

        rawAxis = rawAxis.sqrMagnitude > axisDeadzone * axisDeadzone ? rawAxis.normalized : Vector2.zero;

        timedMagnitude += 1f / (rawAxis != Vector2.zero ? accelerationTime : -decelerationTime) * Time.deltaTime;

        timedMagnitude = Mathf.Clamp01(timedMagnitude);

        timedAxis = rawAxis != Vector2.zero ?
         rawAxis.normalized * timedMagnitude :
         timedAxis.normalized * timedMagnitude;
    }

    void DoFeetRotation()
    {
        playerToLookAtTransform = lookAtTransform.position - transform.position;

        forwardToAimAngle = Vector3.SignedAngle(transform.forward, playerToLookAtTransform, Vector3.up);
        forwardToAimAngle -= curAngleOffset;
        absoluteForwardToAimAngle = Mathf.Abs(forwardToAimAngle);

        if (rb.velocity.sqrMagnitude > rbVelocityDead * rbVelocityDead)
        {
            if (playerShoot.IsAiming)
            {
                transform.LookAt(playerShoot.TargetPos);
            }
            else
            {
                transform.LookAt(transform.position + rb.velocity);
            }

            runningBackward = Vector3.Angle(transform.forward, rb.velocity) > 90f;
        }
        else //we are not moving
        {
            runningBackward = false;
            //scaler.localRotation = Quaternion.identity;

            if (absoluteForwardToAimAngle > 60f)
            {
                Vector3 eulerRotationToDo;
                eulerRotationToDo.x = 0f;
                eulerRotationToDo.y = ((absoluteForwardToAimAngle - 60f) * Mathf.Sign(forwardToAimAngle));
                eulerRotationToDo.z = 0f;
            }
            runningBackward = false;
        }
    }
}
