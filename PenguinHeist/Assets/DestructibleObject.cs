using System;
using UnityEngine;
using UnityEngine.UI;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    public int maxHealth;
    public int currentHealth;
    public GameObject myCanvas;
    public Slider lifeSlider;
    public Image sliderImage;
    public bool getHurt;

    [Space]
    public float timingCanvasVisible = 5f;
    public float timePassedWithCanvas;
    
    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;
        lifeSlider.maxValue = maxHealth;
        lifeSlider.value = currentHealth;
        myCanvas.SetActive(false);
    }

    private void Update()
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
        Debug.Log(currentHealth);
        Debug.Log(maxHealth);
        sliderImage.color = Color.Lerp(Color.red, Color.green, (float)currentHealth/maxHealth);
        
        if (currentHealth > 0) return;
        Destruct();
    }

    private void ResetCanvas(bool isActive)
    {
        getHurt = isActive;
        myCanvas.SetActive(getHurt);
        timePassedWithCanvas = 0f;
    }

    private void Destruct()
    {
        Debug.Log($"{gameObject.name} destroyed");
        gameObject.SetActive(false);
    }

    [ContextMenu("DoDamage5")]
    public void Damage5() => TakeDamage(5);
    
    [ContextMenu("DoDamage20")]
    public void Damage20() => TakeDamage(20);
    
}