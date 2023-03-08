using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    bool isAiming = false;
    public bool IsAiming => isAiming;

    Vector3 targetPos = Vector3.zero;
    public Vector3 TargetPos => targetPos;

    Vector2 rawAimAxis = Vector2.zero;

    [SerializeField] WeaponSO curWeapon;

    int curMagazineBullets = 0;
    int curTotalBullets = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitWeapon(curWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessAxes();

        if (rawAimAxis.magnitude > 0.1f)
        {
            isAiming = true;
            targetPos = transform.position + rawAimAxis.NormalizeIfGreater().Flatten() * 5f;
        }
        else
        {
            isAiming = false;
            targetPos = transform.position;
        }



    }

    void ProcessAxes()
    {
        rawAimAxis.x = Input.GetAxisRaw("Controller Right Horizontal");
        rawAimAxis.y = Input.GetAxisRaw("Controller Right Vertical");
    }

    public void InitWeapon(WeaponSO _weapon)
    {
        curWeapon = _weapon;
        curMagazineBullets = curWeapon.bulletsPerMagazine;
        curTotalBullets = curWeapon.totalBulletsOnPickup;
    }

}
