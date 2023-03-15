using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{

    [SerializeField] PlayerAnimationController animationController;

    [SerializeField] GameObject moneyBagPrefab;

    [SerializeField] Transform launchPoint;
    [SerializeField] Vector2 minMaxLaunchForce = new Vector2(2.5f, 4.5f);
    [SerializeField] Vector2 minMaxLaunchTorque = new Vector2(30f, 60f);

    bool isCarrying = false;
    public bool IsCarrying => isCarrying;

    [SerializeField] float minLaunchTime = 0.2f;


<<<<<<< Updated upstream

    bool canLaunch = false;
=======
    public AudioSource launchSfx;

    private void Start()
    {
        var playerIndex = GetComponent<PlayerController2>().playerIndex;
        interractInput = playerIndex switch
        {
            0 => $"Interract",
            1 => $"InterractP2",
            _ => interractInput
        };
    }
>>>>>>> Stashed changes

    void Update()
    {
        if (isCarrying)
        {
            if (canLaunch && Input.GetKeyDown(KeyCode.G))
            {
                LaunchBag();
            }
        }
    }

    public void CarryBag(MoneyBag _moneyBag)
    {
        isCarrying = true;
        canLaunch = false;
        StartCoroutine(WaitAndCanLaunch());
    }

    void LaunchBag()
    {
        isCarrying = false;
        animationController.LaunchBag();
        
        launchSfx.Play();

        Rigidbody rb = Instantiate(moneyBagPrefab, launchPoint.position, Quaternion.identity).GetComponent<Rigidbody>();

        rb.velocity = launchPoint.forward * Random.Range(minMaxLaunchForce.x, minMaxLaunchForce.y);
        rb.AddTorque(Random.onUnitSphere * Random.Range(minMaxLaunchTorque.x, minMaxLaunchTorque.y));
    }

    IEnumerator WaitAndCanLaunch()
    {
        yield return new WaitForSeconds(minLaunchTime);
        canLaunch = true;
    }
}
