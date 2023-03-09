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
    [Range(0, 10)]
    public int shotsPerClick = 1;
    public float timeBetweenShots = 0.1f;

    public GameObject bulletPrefab = null;

    void OnValidate()
    {
        bulletsPerMagazine.ClampAtZero();
        totalBulletsOnPickup.ClampAtZero();
        reloadTime.ClampAtZero();
        fireRate.ClampAtZero();
        timeBetweenShots.ClampAtZero();
    }

}