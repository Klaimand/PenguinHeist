using System.Collections;
using System.Collections.Generic;
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
    public int playerIndex;
    public string yInput;
    public float timer;
    public bool canInput;
    public Slider heistSlider;
    public List<PlayerController2> playerInRange;
    public ParticleSystem psChest;
    [Space(10)] [Header("On Safe Open")] public UnityEvent OnSafeOpenEvent;
    
    #region Events Methods
    public void Start()
    {
        yInput = playerIndex switch
        {
            0 => $"Change Color",
            1 => $"Change ColorP2",
        };

        heistSlider.maxValue = 100f;
        heistSlider.minValue = 0f;
        heistSlider.value = heistSlider.minValue;
        
        heistCanvas.SetActive(false);
        isOpen = false;
    }

    private void Update()
    {
        if(isOpen) return;
        InputOpenChest();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isOpen) return;
        
        playerInRange.Add(other.GetComponent<PlayerController2>());
        if (playerInRange.Count > 0)
        {
            canInput = true;
            heistCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isOpen) return;
        
        playerInRange.Remove(other.GetComponent<PlayerController2>());
        if (playerInRange.Count < 1)
        {
            canInput = false;
            heistCanvas.SetActive(false);
        }
    }

    #endregion


    #region Methods
    
    void InputOpenChest()
    {
        if (!this.isOpen && Input.GetButtonDown(yInput) && canInput)
        {
            chestGauje += 5;
        }

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
    }

    IEnumerator OpenAndReward()
    {
        myAnimator.SetTrigger("openTrigger"); // Animation du coffre qui s'ouvre
        yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length); // Attendre le temps de l'anim
        GivePlayerMoney();
    }

    private void GivePlayerMoney()
    {
        psChest.Play();
        GameObject GO = Instantiate(bagPrefab, transform.position + Vector3.forward * 2 + Vector3.up, Quaternion.identity);
        Rigidbody rb = GO.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Random.Range(2.5f,3.5f));
        rb.AddTorque(Random.onUnitSphere * Random.Range(100, 300));
    }

    #endregion
}