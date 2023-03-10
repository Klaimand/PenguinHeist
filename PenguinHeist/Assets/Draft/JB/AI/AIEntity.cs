using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AIStateManager))]
public class AIEntity : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] AIStateManager aiStateManager;
    [HideInInspector] public int curMagazineBullets;
    [HideInInspector] public bool isReloading;
    [HideInInspector] public float currentAttackCd;
    [SerializeField] Vector2 minMaxLaunchTorque = new Vector2(100f, 200f);

    private void Start()
    {
        aiStateManager.weaponData = weaponData;
    }

    [ContextMenu("Die")]
    void Die()
    {
        DropWeapon();
        Destroy(transform.parent.gameObject);
    }

    void DropWeapon()
    {
        //drop weapon
        Weapon oldWeapon = Instantiate(weaponData.weaponPrefab, transform.position, Quaternion.identity).GetComponent<Weapon>();
        oldWeapon.Init(weaponData.bulletsPerMagazine, weaponData.totalBulletsOnPickup);

        Rigidbody rb = oldWeapon.GetComponent<Rigidbody>();

        rb.velocity = transform.forward;
        rb.AddTorque(Random.onUnitSphere * Random.Range(minMaxLaunchTorque.x, minMaxLaunchTorque.y));
    }
    
    
}
