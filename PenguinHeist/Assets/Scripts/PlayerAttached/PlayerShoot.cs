using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    Transform canon;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerMelee melee;
    [SerializeField] PlayerBag playerBag;
    [SerializeField] PlayerHealth playerHealth;

    [SerializeField] float aimDeadzone = 0.1f;
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
    public bool IsReloading => isReloading;
    bool canShoot = false;

    //timers
    float timeSinceLastClick = 0f;
    float timeSinceLastBullet = 0f;

    Coroutine reloadCoroutine;


    private string lookInputHorizontalAxis;
    private string lookInputVerticalAxis;
    private string rTriggerInput;

    public TextMeshProUGUI testText;
    public float baseSize;

    public Ease startShootTextEase;
    public Ease EndShootTextEase;

    public AudioSource gunshotSfx;
    public AudioSource reloadSfx;

    [ContextMenu("TextTest")]
    public void TextTest()
    {
        if (testText == null) return;

        testText.transform.DOScale(transform.localScale.x - 0.35f, 0.075f).SetEase(Ease.InBack).OnComplete(() =>
        {
            testText.transform.DOScale(baseSize, 0.75f).SetEase(Ease.OutBounce);
        });
    }

    public Action OnPlayerShoot;
    public Action OnPlayerChangeWeapon;


    // Start is called before the first frame update
    void Start()
    {
        if (curWeapon != null) InitWeapon(curWeapon);

        baseSize = transform.localScale.x;
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

        ProcessCanShootForNextFrame();
    }

    void ProcessInputs()
    {
        rawAimAxis.x = Input.GetAxisRaw(lookInputHorizontalAxis);
        rawAimAxis.y = Input.GetAxisRaw(lookInputVerticalAxis);

        rawAimAxis.ZeroIfBelow(aimDeadzone);

        rightTriggerAxis = Input.GetAxisRaw(rTriggerInput);

        isPressingShootInput = rightTriggerAxis > shootTriggerDeadzone || Input.GetKey(KeyCode.Space);
    }

    void InitWeapon(WeaponSO _weapon)
    {
        curWeapon = _weapon;
        curMagazineBullets = curWeapon.bulletsPerMagazine;
        curTotalBullets = curWeapon.totalBulletsOnPickup;

        OnPlayerChangeWeapon?.Invoke();
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

        OnPlayerChangeWeapon?.Invoke();
    }

    void ProcessCanShootForNextFrame()
    {
        canShoot = !isReloading;
    }

    void CheckShoot()
    {
        if (!canShoot) return;

        if (playerBag.IsCarrying) return;

        if (melee.IsAttacking) return;

        if (playerInteraction.IsInteracting) return;

        if (isReloading) return;

        if (!isPressingShootInput) return;

        if (timeSinceLastClick < curWeapon.fireRate) return;

        if (playerHealth.IsNotAlive) return;

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
                //TextTest();
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

        if (curWeapon.gunshotSFX != null)
        {
            gunshotSfx.clip = curWeapon.gunshotSFX;
            gunshotSfx.Play();
        }
        
        Instantiate(curWeapon.bulletPrefab, canon.position, Quaternion.LookRotation(dir));

        if (curWeapon.muzzleFlash != null)
            Instantiate(curWeapon.muzzleFlash, canon.position, Quaternion.LookRotation(dir));

        OnPlayerShoot?.Invoke();
        GameManager.instance.EventsManager.TriggerEvent("OnPlayerShoot");

        //Debug.DrawRay(canon.position, dir, Color.green, 0.3f);
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(curWeapon.reloadTime);
        curMagazineBullets = Mathf.Min(curWeapon.bulletsPerMagazine, curTotalBullets);
        curTotalBullets -= curMagazineBullets;
        isReloading = false;
        reloadSfx.Play();

        GameManager.instance.EventsManager.TriggerEvent("OnPlayerReloadEnd");
    }

    void IncreaseTimers()
    {
        timeSinceLastClick += Time.deltaTime;
        timeSinceLastBullet += Time.deltaTime;
    }

    private void OnEnable()
    {
        OnPlayerChangeWeapon += ChangeUIWeapon;
    }

    private void OnDisable()
    {
        OnPlayerChangeWeapon -= ChangeUIWeapon;
    }


    public Image weaponImage;
    void ChangeUIWeapon()
    {
        if (playerBag.IsCarrying)
        {
            weaponImage.sprite = playerBag.BagSprite;
            return;
        }
        
        weaponImage.sprite = curWeapon.weaponImage;
    }
}