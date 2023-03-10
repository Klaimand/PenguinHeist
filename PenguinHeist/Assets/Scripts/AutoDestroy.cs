using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 1f;

    void OnValidate()
    {
        if (timeToDestroy < 0f) timeToDestroy = 0f;
    }

    void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

}
