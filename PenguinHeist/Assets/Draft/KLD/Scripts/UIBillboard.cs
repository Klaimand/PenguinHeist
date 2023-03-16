using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    [SerializeField] Transform camTransform;

    
    
    private void Start()
    {
        if (camTransform == null) camTransform = Camera.main.transform;
        //print(camTransform.gameObject.name);
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + camTransform.forward, Vector3.up);
    }
}
