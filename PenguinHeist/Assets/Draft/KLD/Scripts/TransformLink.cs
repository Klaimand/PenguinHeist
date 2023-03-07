using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_TransformLink : MonoBehaviour
{
    [SerializeField] Transform linkTo = null;
    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField] bool ignoreY = false;
    [SerializeField] bool smoothTranslation = false;
    [SerializeField] float smoothness = 0.2f;

    [SerializeField] bool lockRotation = false;
    [SerializeField] Vector3 angleOffset = Vector3.zero;
    [SerializeField] bool lockX = false, lockY = false, lockZ = false;

    Vector3 angles = Vector3.zero;
    Vector3 desiredPos = Vector3.zero;

    void Start()
    {
        transform.position = linkTo.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (linkTo == null)
        {
            Debug.LogError("Transform Link missing transform");
            return;
        }

        if (!smoothTranslation)
        {
            //transform.position = linkTo.position + offset;

            desiredPos = linkTo.position + offset;

            if (ignoreY) desiredPos.y = transform.position.y;

            transform.position = desiredPos;
        }
        else
        {
            desiredPos.x = linkTo.position.x;
            desiredPos.y = ignoreY ? transform.position.y : linkTo.position.y;
            desiredPos.z = linkTo.position.z;

            transform.position = Vector3.Lerp(transform.position, desiredPos + offset, smoothness);
        }

        if (lockRotation)
        {
            angles.x = lockX ? linkTo.rotation.eulerAngles.x + angleOffset.x : transform.rotation.eulerAngles.x;
            angles.y = lockY ? linkTo.rotation.eulerAngles.y + angleOffset.y : transform.rotation.eulerAngles.y;
            angles.z = lockZ ? linkTo.rotation.eulerAngles.z + angleOffset.z : transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(angles);
        }
    }
}