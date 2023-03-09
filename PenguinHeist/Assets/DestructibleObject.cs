using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private Image sliderImage;
    
    [Header("Object Setting")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    
    [Space]
    [Header("Feebacks")]
    [SerializeField] private float timingCanvasVisible = 4.5f;
    [SerializeField] private float sliderOpenSize = 0.35f;
    [SerializeField] private float sliderCloseSize = 0f;
    [SerializeField] private float objectFeedbackOffsetSize = 0.15f;
    
    
    private bool getHurt;
    private float timePassedWithCanvas;
    private float objectSize;
    
    // Start is called before the first frame update
    private void Start()
    {
        objectSize = transform.localScale.x;
        getHurt = false;
        currentHealth = maxHealth;
        lifeSlider.maxValue = maxHealth;
        lifeSlider.value = currentHealth;
        lifeSlider.transform.DOScale(sliderCloseSize, 0.01f);
    }

    private void FixedUpdate()
    {
        if (!getHurt) return;
        
        timePassedWithCanvas += Time.deltaTime;
        if (timePassedWithCanvas > timingCanvasVisible)
        {
            ResetCanvas(false);
        }
    }

    public void TakeDamage(int _damage)
    {
        ResetCanvas(true);
        currentHealth -= _damage;
        lifeSlider.value = currentHealth;
        sliderImage.color = Color.Lerp(Color.red, Color.green, (float)currentHealth/maxHealth);
        if (currentHealth > 0) return;
        Destruct();
    }

    private void ResetCanvas(bool isActive)
    {
        getHurt = isActive;
        timePassedWithCanvas = 0f;

        if (getHurt)
        {
            lifeSlider.transform.DOKill();
            lifeSlider.transform.DOScale(sliderOpenSize, .55f).SetEase(Ease.OutSine);
            
            transform.DOKill();
            transform.DOScale(objectSize + objectFeedbackOffsetSize, 0.185f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                transform.DOScale(objectSize, 0.1f).SetEase(Ease.InBack);
            });
        }
        else
        {
            lifeSlider.transform.DOKill();
            lifeSlider.transform.DOScale(sliderCloseSize, 0.175f).SetEase(Ease.InBack);
        }
    }

    private void Destruct()
    {
        Debug.Log($"{gameObject.name} destroyed");
        gameObject.SetActive(false);
        // TODO Add fx 
    }

    [ContextMenu("DoDamage10")]
    public void Damage10() => TakeDamage(10);
}