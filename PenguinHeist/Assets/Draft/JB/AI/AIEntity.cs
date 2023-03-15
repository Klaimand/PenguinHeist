using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AIStateManager))]
public class AIEntity : MonoBehaviour, IDamageable
{
    [SerializeField] private WeaponSO weaponData;
    public WeaponSO Weapon => weaponData;
    [SerializeField] public AIStateManager aiStateManager;
    [HideInInspector] public int curMagazineBullets;
    [HideInInspector] public bool isReloading; // In AIAttackState
    public bool isHoldingShield = false; // In Shield
    [HideInInspector] public float currentAttackCd;
    [Tooltip("Launch force for the weapon when it drops")]
    [SerializeField] Vector2 minMaxLaunchTorque = new Vector2(100f, 200f);
    [SerializeField] private float health = 30;
    [SerializeField] Collider col;

    bool isDead = false;
    public bool IsDead => isDead;

    public Action OnHit;

    private void Start()
    {
        //isHoldingShield = true;
        aiStateManager.weaponData = weaponData;
    }

    [ContextMenu("Die")]
    void Die()
    {
        if (isDead) return;

        col.enabled = false;

        isDead = true;
        if (aiStateManager.aiType == AIType.Police)
        {
            LevelManager.instance.RemovePoliceEnemy(aiStateManager);
        }
        else
        {
            LevelManager.instance.RemoveMafiaEnemy(aiStateManager);
        }
        aiStateManager.aIStateType = AIStateType.Death;
        DropWeapon();
        //Destroy(transform.gameObject);
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
        //Hit
        if (isDead) return;

        OnHit?.Invoke();
        health -= _damage;
        if (health <= 0)
        {
            Die();
        }
    }
}
