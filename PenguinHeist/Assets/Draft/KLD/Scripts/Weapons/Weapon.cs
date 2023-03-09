using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponSO weaponSO;
    public WeaponSO WeaponSO => weaponSO;

    [SerializeField] bool autoInit = false;

    int curBullets = 0;
    public int CurBullets => curBullets;
    int curTotalBullets = 0;
    public int CurTotalBullets => curTotalBullets;

    void Start()
    {
        if (autoInit) Init();
    }

    public void Init()
    {
        curBullets = weaponSO.bulletsPerMagazine;
        curTotalBullets = weaponSO.totalBulletsOnPickup;
    }

    public void Init(int _curBullets, int _curTotalBullets)
    {
        curBullets = _curBullets;
        curTotalBullets = _curTotalBullets;
    }

    void OnDestroy()
    {
        GameManager.instance.EventsManager.TriggerEvent("OnWeaponDestroy");
    }
}
