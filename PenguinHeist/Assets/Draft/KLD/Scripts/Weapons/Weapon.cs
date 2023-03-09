using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponSO weaponSO;
    public WeaponSO WeaponSO => weaponSO;

    int curBullets = 0;
    int curTotalBullets = 0;


    public void Init(WeaponSO _weapon, int _curBullets, int _curTotalBullets)
    {
        weaponSO = _weapon;
        curBullets = _curBullets;
        curTotalBullets = _curTotalBullets;
    }
}
