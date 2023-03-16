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
    public Sprite BagSprite;
    
    bool isCarrying = false;
    public bool IsCarrying => isCarrying;

    [SerializeField] float minLaunchTime = 0.2f;


    public string interractInput;
    
    bool canLaunch = false;
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
    }

    void LaunchBag()
    {
        isCarrying = false;
        animationController.LaunchBag();
        
        launchSfx.Play();

        Rigidbody rb = Instantiate(moneyBagPrefab, launchPoint.position, Quaternion.identity).GetComponent<Rigidbody>();

        rb.velocity = launchPoint.forward * Random.Range(minMaxLaunchForce.x, minMaxLaunchForce.y);
        rb.AddTorque(Random.onUnitSphere * Random.Range(minMaxLaunchTorque.x, minMaxLaunchTorque.y));
        LevelManager.instance.EnemyTakeBag(rb.transform);
    }

    IEnumerator WaitAndCanLaunch()
    {
        yield return new WaitForSeconds(minLaunchTime);
        canLaunch = true;
    }
}
