using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AIStateManager))]
public class AIEntity : MonoBehaviour, IDamageable
{
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] public AIStateManager aiStateManager;
    [HideInInspector] public int curMagazineBullets;
    [HideInInspector] public bool isReloading;
    [HideInInspector] public float currentAttackCd;
    [Tooltip("Launch force for the weapon when it drops")]
    [SerializeField] Vector2 minMaxLaunchTorque = new Vector2(100f, 200f);
    [SerializeField] private float health = 30;

    private void Start()
    {
        aiStateManager.weaponData = weaponData;
    }

    [ContextMenu("Die")]
    void Die()
    {
        aiStateManager.aIStateType = AIStateType.Death;
        DropWeapon();
        Destroy(transform.gameObject);
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


    public void TakeDamage(int _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            Die();
        }
        aiStateManager.aIStateType = AIStateType.Hit;
    }
}
