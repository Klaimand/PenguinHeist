using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Safe : MonoBehaviour
{
    public bool isOpen;
    public GameObject bagPrefab;
    public GameObject heistCanvas;
    public Animator myAnimator;
    public SphereCollider myCollider;
    public int chestGauje;
    public float timer;
    public bool canInput;
    public Slider heistSlider;
    public List<PlayerOpenChest> playerInRange;
    public ParticleSystem psChest;
    public ParticleSystem psChestIsHere;
    [Space(10)][Header("On Safe Open")] public UnityEvent OnSafeOpenEvent;

    #region Events Methods
    public void Start()
    {
        heistSlider.maxValue = 100f;
        heistSlider.minValue = 0f;
        heistSlider.value = heistSlider.minValue;
        heistCanvas.SetActive(false);
        psChestIsHere.Play();
        isOpen = false;
    }

    public bool inputPressed;
    private void Update()
    {
        if (isOpen) return;
        if (inputPressed)
        {
            InputOpenChest();
            inputPressed = false;
        }

        DecreasePoints();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isOpen) return;
        var player = other.GetComponent<PlayerOpenChest>();

        playerInRange.Add(player);
        if (playerInRange.Count > 0)
        {
            canInput = true;
            heistCanvas.SetActive(true);
            player.currentSafe = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isOpen) return;
        var player = other.GetComponent<PlayerOpenChest>();
        playerInRange.Remove(player);

        if (playerInRange.Count < 1)
        {
            canInput = false;
            heistCanvas.SetActive(false);
            player.currentSafe = null;
        }
    }


    #endregion


    #region Methods

    public void InputOpenChest()
    {
        if (!this.isOpen && canInput)
        {
            chestGauje += 3;
            FeedBackTweening();
        }
    }

    public void DecreasePoints()
    {
        if (chestGauje < 1) return;

        timer += Time.deltaTime;
        if (timer > 0.25f)
        {
            chestGauje -= 1;
            timer = 0;
        }

        heistSlider.value = chestGauje;

        if (chestGauje <= 99) return;

        Debug.Log("IZI MONEY BOOBA IS PROUD");
        OpenSafe();
    }

    private void OpenSafe()
    {
        myCollider.enabled = false;
        heistCanvas.SetActive(false);
        isOpen = true;
        StartCoroutine(OpenAndReward());
        OnSafeOpenEvent.Invoke();
    }

    IEnumerator OpenAndReward()
    {
        myAnimator.SetTrigger("openTrigger"); // Animation du coffre qui s'ouvre
        yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length); // Attendre le temps de l'anim
        GivePlayerMoney();
    }

    public Transform bagSpawnPoint;
    private void GivePlayerMoney()
    {
        psChest.Play();
        GameObject GO = Instantiate(bagPrefab, bagSpawnPoint.position + transform.forward, Quaternion.identity);
        GO.transform.DOScale(0, 0.001f).OnComplete(() =>
            GO.transform.DOScale(1, 1.15f).SetEase(Ease.OutElastic));
        Rigidbody rb = GO.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Random.Range(2.5f * 3, 3.5f * 3.5f));
        rb.AddTorque(Random.onUnitSphere * Random.Range(100, 300));
        Destroy(psChestIsHere.gameObject);
    }

    public Image _image;
    public RectTransform _Rect;
    public void FeedBackTweening()
    {
        _image.rectTransform.DOShakePosition(0.185f, new Vector3(1, 1, 1), 35);
    }

    #endregion
}