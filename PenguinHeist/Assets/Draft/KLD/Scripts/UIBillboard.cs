using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    Transform camTransform;

    private void Start()
    {
        camTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + camTransform.forward, Vector3.up);
    }
}
