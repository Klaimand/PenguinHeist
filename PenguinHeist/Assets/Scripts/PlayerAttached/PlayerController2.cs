using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController2 : MonoBehaviour
{
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

    float refRotVelo = 0f;

    public float Speed => rb.velocity.magnitude;

    // Start is called before the first frame update
    void Start()
    {

    }

    int a = 0;
    // Update is called once per frame
    void Update()
    {
        ProcessAxes();

        rb.velocity = smoothedAxis.Flatten() * speed;

        DoRotation();
    }

    void ProcessAxes()
    {
        rawAxis.x = Input.GetAxisRaw("Horizontal");
        rawAxis.y = Input.GetAxisRaw("Vertical");

        rawAxis.ZeroIfBelow(axisDeadzone);
        rawAxis.NormalizeIfGreater();

        smoothedAxis = Vector2.SmoothDamp(smoothedAxis, rawAxis, ref refAxisVelocity, axisSmoothTime, axisMaxSpeed);
    }

    void DoRotation()
    {
        Quaternion wantedRotation = transform.rotation;

        Vector3 playerToTargetPos = playerShoot.TargetPos - transform.position;

        if (playerShoot.IsAiming)
        {
            wantedRotation = Quaternion.LookRotation(playerToTargetPos);
        }
        else if (rb.velocity.magnitude > rbVelocityDead)
        {
            wantedRotation = Quaternion.LookRotation(rb.velocity.normalized);
        }

        Quaternion rot = Quaternion.Euler(0f,
        Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, wantedRotation.eulerAngles.y, ref refRotVelo, rotSmoothTime, rotMaxSpeed),
        0f);

        transform.rotation = rot;
    }
}
