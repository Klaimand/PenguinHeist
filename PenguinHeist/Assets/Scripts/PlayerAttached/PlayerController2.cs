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
