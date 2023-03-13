using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] LayerMask interactionLayers;

    [SerializeField] Vector3 localCastOrigin = Vector3.zero;
    [SerializeField] float castRadius = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Physics.SphereCastAll()
    }
}
