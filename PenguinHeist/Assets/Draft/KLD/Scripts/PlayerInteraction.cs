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

    PlayerHealth playerHealth;

    public string interractInput;

    public AudioSource pickupSfx;

    [SerializeField] GameObject reviveCanvas;
    [SerializeField] GameObject moneyPickupCanvas;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Start is called before the first frame update
    void Start()
    {
        reviveCanvas.SetActive(false);
        moneyPickupCanvas.SetActive(false);
        var playerIndex = GetComponent<PlayerController2>().playerIndex;
        interractInput = playerIndex switch
        {
            0 => $"Interract",
            1 => $"InterractP2",
            _ => interractInput
        };
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

        bool isDetectingRevive = false;
        bool isDetectingWeapon = false;

        for (int i = 0; i < cols.Length; i++)
        {
            IInteractible ii = cols[i].GetComponent<IInteractible>();
            
            if (!playerHealth.IsNotAlive && ii.IsInteractable() && ii.GetInteractionType() == InteractionType.REVIVE)
            {
                isDetectingRevive = true;
                reviveCanvas.SetActive(true);
                break;
            }
            
            if (!playerHealth.IsNotAlive && ii.IsInteractable() && ii.GetInteractionType() == InteractionType.MONEY_BAG)
            {
                isDetectingWeapon = true;
                moneyPickupCanvas.SetActive(true);
                break;
            }
        }

        if (!isDetectingRevive) reviveCanvas.SetActive(false);
        if (!isDetectingWeapon) moneyPickupCanvas.SetActive(false);

        if (!playerBag.IsCarrying && Input.GetButtonDown(interractInput))
        {
            for (int i = 0; i < cols.Length; i++)
            {
                IInteractible ii = cols[i].GetComponent<IInteractible>();

                if (ii.IsInteractable())
                {
                    if (ii.GetInteractionDuration() > 0f)
                    {
                        if (pickupSfx != null)
                            pickupSfx.Play();

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
