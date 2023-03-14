using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] PlayerBag playerBag;
    public PlayerBag PplayerBag => playerBag;

    [SerializeField] LayerMask interactionLayers;

    [SerializeField] float fwdCastDistance = 0.4f;
    [SerializeField] float castRadius = 0.77f;

    bool isDetectingInteraction = false;

    bool isInteracting = false;
    public bool IsInteracting => isInteracting;

    public Action OnPlayerInteract;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward * fwdCastDistance, castRadius, interactionLayers);

        if (cols.Length == 0)
        {
            isDetectingInteraction = false;
            return;
        }

        isDetectingInteraction = true;

        if (!playerBag.IsCarrying && Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < cols.Length; i++)
            {
                IInteractible ii = cols[i].GetComponent<IInteractible>();

                if (ii.IsInteractable())
                {
                    if (ii.GetInteractionDuration() > 0f)
                    {
                        ii.InteractImmediate(this);
                        isInteracting = true;
                        StartCoroutine(WaitAndInteract(ii.GetInteractionDuration(), ii));
                    }
                    else
                    {
                        ii.Interact(this);
                        OnPlayerInteract?.Invoke();
                    }
                    break;
                }
            }

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * fwdCastDistance, castRadius);
    }

    IEnumerator WaitAndInteract(float _d, IInteractible _ii)
    {
        yield return new WaitForSeconds(_d);
        _ii.Interact(this);
        isInteracting = false;
    }
}
