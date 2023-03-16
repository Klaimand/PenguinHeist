using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    [Header("Object Setting")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [Space]
    [Header("Feebacks")]
    [SerializeField] private float objectFeedbackOffsetSize = 0.15f;
    
    [Header("GameObject FB")]
    [SerializeField] private Ease enterGOEase;
    [SerializeField] private float openGODuration;
    [SerializeField] private Ease exitGOEase;
    [SerializeField] private float exitGODuration;

    [SerializeField] UnityEvent onObjectTakeDamage;
    [SerializeField] UnityEvent onObjectDestroy;

    private bool getHurt;
    private float objectSize;

    public AudioSource touchedSfx;
    public AudioSource breakSfx;

    // Start is called before the first frame update
    private void Start()
    {
        objectSize = transform.localScale.x;
        getHurt = false;
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int _damage)
    {
        transform.DOScale(objectSize + objectFeedbackOffsetSize, openGODuration).SetEase(enterGOEase).OnComplete(() =>
        {
            transform.DOScale(objectSize, exitGODuration).SetEase(exitGOEase);
        });
        
        touchedSfx.Play();
        currentHealth -= _damage;
        onObjectTakeDamage.Invoke();
        if (currentHealth > 0) return;
        Destruct();
    }
    
    private void Destruct()
    {
        breakSfx.Play();
        onObjectDestroy.Invoke();
    }
}