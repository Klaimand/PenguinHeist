using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")] [SerializeField]
    Transform canon;

    [SerializeField] float shootTriggerDeadzone = 0.1f;

    bool isAiming = false;
    public bool IsAiming => isAiming;

    Vector3 targetPos = Vector3.zero;
    public Vector3 TargetPos => targetPos;

    Vector2 rawAimAxis = Vector2.zero;
    float rightTriggerAxis = 0f;
    bool isPressingShootInput = false;

    [SerializeField] WeaponSO curWeapon;
    public WeaponSO CurWeapon => curWeapon;

    int curMagazineBullets = 0;
    int curTotalBullets = 0;

    public int CurMagazineBullets => curMagazineBullets;
    public int CurTotalBullets => curTotalBullets;

    bool isReloading = false;

    //timers
    float timeSinceLastClick = 0f;
    float timeSinceLastBullet = 0f;

    Coroutine reloadCoroutine;

    private string lookInputHorizontalAxis;
    private string lookInputVerticalAxis;
    private string rTriggerInput;

    public TextMeshProUGUI testText;
    public float baseSize;

    [ContextMenu("TextTest")]
    public void TextTest()
    {
        testText.transform.DOScale(transform.localScale.x - 0.15f, 0.15f ).OnComplete(() =>
                testText.transform.DOScale(baseSize, 0.75f));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        baseSize = transform.localScale.x;
        if (curWeapon != null) InitWeapon(curWeapon);

        var playerController = GetComponent<PlayerController2>();
        
        if (playerController.playerIndex == 0)
        {
            lookInputHorizontalAxis = $"Controller Right Horizontal";
            lookInputVerticalAxis = $"Controller Right Vertical";
            rTriggerInput = $"Right Trigger";
        }

        if (playerController.playerIndex == 1)
        {
            lookInputHorizontalAxis = $"Controller Right HorizontalP2";
            lookInputVerticalAxis = $"Controller Right VerticalP2";
            rTriggerInput = $"Right TriggerP2";
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();

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

        CheckShoot();

        IncreaseTimers();
    }

    void ProcessInputs()
    {
        rawAimAxis.x = Input.GetAxisRaw(lookInputHorizontalAxis);
        rawAimAxis.y = Input.GetAxisRaw(lookInputVerticalAxis);
        rightTriggerAxis = Input.GetAxisRaw(rTriggerInput);

        isPressingShootInput = rightTriggerAxis > shootTriggerDeadzone || Input.GetKey(KeyCode.Space);
    }

    void InitWeapon(WeaponSO _weapon)
    {
        curWeapon = _weapon;
        curMagazineBullets = curWeapon.bulletsPerMagazine;
        curTotalBullets = curWeapon.totalBulletsOnPickup;
    }

    public void InitWeapon(WeaponSO _weapon, int _curBulletsInMag, int _curTotalBullets)
    {
        if (isReloading && reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            isReloading = false;
        }

        curWeapon = _weapon;
        curMagazineBullets = _curBulletsInMag;
        curTotalBullets = _curTotalBullets;
    }

    void CheckShoot()
    {
        if (isReloading) return;

        if (!isPressingShootInput) return;

        if (timeSinceLastClick < curWeapon.fireRate) return;

        timeSinceLastClick = 0f;
        StartCoroutine(DoBurstShoot());
    }

    IEnumerator DoBurstShoot()
    {
        for (int i = 0; i < curWeapon.shotsPerClick; i++)
        {
            if (curMagazineBullets > 0)
            {
                Shoot();
                TextTest();
                timeSinceLastBullet = 0f;
            }
            else if (curTotalBullets > 0)
            {
                if (isReloading) yield break;

                isReloading = true;
                reloadCoroutine = StartCoroutine(Reload());
            }
            else
            {
                //plus de balles
            }

            yield return new WaitForSeconds(curWeapon.timeBetweenShots);
        }

        if (curTotalBullets > 0 && curMagazineBullets == 0)
        {
            if (isReloading) yield break;

            isReloading = true;
            reloadCoroutine = StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        curMagazineBullets--;

        Vector3 dir = canon.forward;
        dir.y = 0f;

        float rdmAngle = Random.Range(-curWeapon.spread / 2f, curWeapon.spread / 2f);

        dir = Quaternion.Euler(0f, rdmAngle, 0f) * dir;

        Instantiate(curWeapon.bulletPrefab, canon.position, Quaternion.LookRotation(dir));

        if (curWeapon.muzzleFlash != null)
            Instantiate(curWeapon.muzzleFlash, canon.position, Quaternion.LookRotation(dir));

        GameManager.instance.EventsManager.TriggerEvent("OnPlayerShoot");

        //Debug.DrawRay(canon.position, dir, Color.green, 0.3f);
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(curWeapon.reloadTime);
        curMagazineBullets = Mathf.Min(curWeapon.bulletsPerMagazine, curTotalBullets);
        curTotalBullets -= curMagazineBullets;
        isReloading = false;

        GameManager.instance.EventsManager.TriggerEvent("OnPlayerReloadEnd");
    }

    void IncreaseTimers()
    {
        timeSinceLastClick += Time.deltaTime;
        timeSinceLastBullet += Time.deltaTime;
    }
}