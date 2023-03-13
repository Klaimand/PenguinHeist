using System;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController2 : MonoBehaviour
{
    public int playerIndex;
    
    [Header("References")]
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerShoot playerShoot;

    [Header("Values")]
    [SerializeField] float axisDeadzone = 0.1f;
    [SerializeField] float speed = 8f;

    Vector2 rawAxis = Vector2.zero;
    Vector2 smoothedAxis = Vector2.zero;

    Vector2 refAxisVelocity = Vector2.zero;

    [SerializeField] float axisSmoothTime = 0.05f;
    [SerializeField] float axisMaxSpeed = 1f;
    [SerializeField] float rotationSmoothing = 0.075f;

    [SerializeField] float rotSmoothTime = 0.2f;
    [SerializeField] float rotMaxSpeed = 90f;

    [SerializeField] float rbVelocityDead = 0.2f;


    public string hMove;
    public string vMove;

    float refRotVelo = 0f;

    private void Start()

    [SerializeField]
    float ang = 37.5f;

    float refRotVelo = 0f;

    public float Speed => rb.velocity.magnitude;

    bool runningBackward = false;
    public bool RunningBackward => runningBackward;

    float aimAngleClamped = 0f;
    public float AimAngleClamped => aimAngleClamped;

    // Start is called before the first frame update
    void Start()

    {
        if (playerIndex == 0)
        {
            hMove = "Horizontal";
            vMove = "Vertical";
        }

        if (playerIndex == 1)
        {
            hMove = "HorizontalP2";
            vMove = "VerticalP2";
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessAxes();

        rb.velocity = smoothedAxis.Flatten() * speed;

        DoRotation();

        Debug.DrawRay(transform.position + Vector3.up * 0.1f,
        (Quaternion.Euler(0f, ang, 0f) * transform.forward) * 5f);


        Debug.DrawRay(transform.position + Vector3.up * 0.1f,
        (Quaternion.Euler(0f, -ang, 0f) * transform.forward) * 5f);
    }

    void ProcessAxes()
    {
        rawAxis.x = Input.GetAxisRaw(hMove);
        rawAxis.y = Input.GetAxisRaw(vMove);

        rawAxis.ZeroIfBelow(axisDeadzone);
        rawAxis.NormalizeIfGreater();

        smoothedAxis = Vector2.SmoothDamp(smoothedAxis, rawAxis, ref refAxisVelocity, axisSmoothTime, axisMaxSpeed); 
    }

    void DoRotation()
    {
        Quaternion wantedRotation = transform.rotation;

        Vector3 playerToTargetPos = playerShoot.TargetPos - transform.position;

        aimAngleClamped = 0.5f;

        runningBackward = false;

        if (rb.velocity.magnitude > rbVelocityDead)
        {
            if (playerShoot.IsAiming)
            {
                wantedRotation = Quaternion.LookRotation(playerToTargetPos);
            }
            else
            {
                wantedRotation = Quaternion.LookRotation(rb.velocity.normalized);
            }

            runningBackward = Vector3.Angle(transform.forward, rb.velocity) > 90f;
        }
        else if (playerShoot.IsAiming)
        {
            Vector3 playerToLookAtTransform = playerShoot.TargetPos - transform.position;

            float forwardToAimAngle = Vector3.SignedAngle(transform.forward, playerToLookAtTransform, Vector3.up);
            //forwardToAimAngle -= curAngleOffset;
            float absoluteForwardToAimAngle = Mathf.Abs(forwardToAimAngle);

            aimAngleClamped = Mathf.InverseLerp(-37.5f, 37.5f, forwardToAimAngle);

            runningBackward = false;

            if (absoluteForwardToAimAngle > 37.5f)
            {
                Vector3 eulerRotationToDo;
                eulerRotationToDo.x = 0f;
                eulerRotationToDo.y = ((absoluteForwardToAimAngle - 37.5f) * Mathf.Sign(forwardToAimAngle));
                eulerRotationToDo.z = 0f;

                //transform.Rotate(eulerRotationToDo);

                wantedRotation = Quaternion.Euler(transform.rotation.eulerAngles + eulerRotationToDo);
            }
        }

        Quaternion rot = Quaternion.Euler(0f,
        Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, wantedRotation.eulerAngles.y, ref refRotVelo, rotSmoothTime, rotMaxSpeed),
        0f);

        transform.rotation = rot;
    }
}
