using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeapon", menuName = "ScriptableObjects/Weapon", order = 0)]
public class WeaponSO : ScriptableObject
{
    [Range(0f, 90f)]
    public float spread = 30f;

    public int bulletsPerMagazine = 30;
    public int totalBulletsOnPickup = 90;
    public float reloadTime = 2.5f;

    public float fireRate = 0.5f;
    [Range(1, 10)]
    public int shotsPerClick = 1;
    [Tooltip("Time between shots when there is more than 1 shot per click")]
    public float timeBetweenShots = 0.1f;

    public GameObject weaponPrefab = null;

    public GameObject bulletPrefab = null;

    void OnValidate()
    {
        bulletsPerMagazine.ClampAtZero();
        totalBulletsOnPickup.ClampAtZero();
        if (totalBulletsOnPickup < bulletsPerMagazine) totalBulletsOnPickup = bulletsPerMagazine;
        reloadTime.ClampAtZero();
        fireRate.ClampAtZero();

        timeBetweenShots = Mathf.Clamp(timeBetweenShots, 0f, fireRate / shotsPerClick);
    }

}