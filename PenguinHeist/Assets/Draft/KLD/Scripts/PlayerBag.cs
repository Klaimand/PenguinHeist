using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField] private GameObject bagUI;

    [SerializeField] private ParticleSystem system;

    public string interractInput;

    bool canLaunch = false;
    public AudioSource launchSfx;

    private void Start()
    {
        bagUI.SetActive(false);
        
        var playerIndex = GetComponent<PlayerController2>().playerIndex;
        interractInput = playerIndex switch
        {
            0 => $"Interract",
            1 => $"InterractP2",
            _ => interractInput
        };
    }

    void Update()
    {
        if (isCarrying)
        {
            if (canLaunch && Input.GetButtonDown(interractInput))
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
        LevelManager.instance.StopEnemyTakeBag(_moneyBag.transform);
        
        bagUI.SetActive(true);
        system.Play();
    }

    void LaunchBag()
    {
        isCarrying = false;
        animationController.LaunchBag();

        if (launchSfx != null)
            launchSfx.Play();

        Rigidbody rb = Instantiate(moneyBagPrefab, launchPoint.position, Quaternion.identity).GetComponent<Rigidbody>();

        rb.velocity = launchPoint.forward * Random.Range(minMaxLaunchForce.x, minMaxLaunchForce.y);
        rb.AddTorque(Random.onUnitSphere * Random.Range(minMaxLaunchTorque.x, minMaxLaunchTorque.y));
        LevelManager.instance.EnemyTakeBag(rb.transform);
        
        bagUI.SetActive(false);
        system.Stop();
    }

    IEnumerator WaitAndCanLaunch()
    {
        yield return new WaitForSeconds(minLaunchTime);
        canLaunch = true;
    }
}
