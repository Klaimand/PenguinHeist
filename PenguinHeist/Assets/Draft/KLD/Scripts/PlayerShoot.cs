using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    bool isAiming = false;
    public bool IsAiming => isAiming;

    public Vector3 TargetPos => targetPos;
    Vector3 targetPos = Vector3.zero;

    Vector2 rawAimAxis = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rawAimAxis.x = Input.GetAxisRaw("Controller Right Horizontal");
        rawAimAxis.y = Input.GetAxisRaw("Controller Right Vertical");

        if (rawAimAxis.magnitude > 0.1f)
        {
            isAiming = true;
            targetPos = transform.position + rawAimAxis.NormalizeIfGreater().Flatten() * 5f;
        }
        else
        {
            isAiming = false;
            targetPos = transform.position;
        }
    }
}
