using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponPickup : MonoBehaviour
{
    [SerializeField] PlayerShoot playerShoot;
    [SerializeField] Transform launchPoint;
    [SerializeField] GameObject pickupCanvas = null;

    //[SerializeField] Vector3 launchDir = Vector3.forward;
    [SerializeField] Vector2 minMaxLaunchForce = new Vector2(1.5f, 3f);

    [SerializeField] Vector2 minMaxLaunchTorque = new Vector2(30f, 60f);

    List<Weapon> weapons = new List<Weapon>();

    bool canPickup = false;
    public string pickUpInput;

    public AudioSource pickupSfx;

    void Start()
    {
        var myIndex = transform.parent.GetComponent<PlayerController2>().playerIndex;
        pickUpInput = myIndex switch
        {
            0 => $"Pickup",
            1 => $"PickupP2",
            _ => pickUpInput
        };

        UpdatePickupState();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Weapon")) return;

        weapons.Add(other.gameObject.GetComponent<Weapon>());

        UpdatePickupState();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Weapon")) return;

        weapons.Remove(other.gameObject.GetComponent<Weapon>());

        UpdatePickupState();
    }

    void Update()
    {
        if (Input.GetButtonDown(pickUpInput))
        {
            if (canPickup)
            {
                pickupSfx.Play();
                
                //drop weapon
                Weapon oldWeapon = Instantiate(playerShoot.CurWeapon.weaponPrefab, launchPoint.position, Quaternion.identity).GetComponent<Weapon>();
                oldWeapon.Init(playerShoot.CurMagazineBullets, playerShoot.CurTotalBullets);

                Rigidbody rb = oldWeapon.GetComponent<Rigidbody>();

                rb.velocity = launchPoint.forward * Random.Range(minMaxLaunchForce.x, minMaxLaunchForce.y);
                rb.AddTorque(Random.onUnitSphere * Random.Range(minMaxLaunchTorque.x, minMaxLaunchTorque.y));

                //take weapon
                Weapon newWeapon = weapons[0];
                playerShoot.InitWeapon(newWeapon.WeaponSO, newWeapon.CurBullets, newWeapon.CurTotalBullets);
                DestroyImmediate(newWeapon.gameObject);

                //trigger event
                GameManager.instance.EventsManager.TriggerEvent("OnPlayerPickupWeapon");
            }
        }
    }

    void UpdatePickupState()
    {
        canPickup = weapons.Count > 0;

        pickupCanvas?.SetActive(canPickup);
    }

    [ContextMenu("Check Weapons")]
    public void CheckWeapons()
    {
        List<int> indexesToRemove = new List<int>();

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] == null || weapons[i].gameObject == null)
            {
                indexesToRemove.Add(i);
            }
        }

        for (int i = indexesToRemove.Count - 1; i >= 0; i--)
        {
            weapons.RemoveAt(indexesToRemove[i]);
        }

        UpdatePickupState();
    }
}
