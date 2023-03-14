using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable, IInteractible
{
    #region Variables

    public int maxHealth = 100;
    public int currentHealth = 0;
    public PlayerState playerstate;

    public float playerKnockOutTimer = 15f;
    private float _knockOutTimer = default;

    public float playerReviveTimer = 7.5f;
    private float _reviveTimer = default;
    
    public float invincibleDuration = 2f;

    public TextMeshProUGUI playerHealthDebugText;
    public TextMeshProUGUI playerStateDebugText;
    

    public bool inReanimation;
    public bool isInvincible;
    
    [Header("Rescue Bar")]
    public GameObject knockOutCanvas;
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private Image sliderImage;
    
    #endregion

    #region Event Methods

    // Start is called before the first frame update
    void Start()
    {
        InitPlayerHealth();
    }

    private void InitPlayerHealth()
    {
        // Health Init Values
        currentHealth = maxHealth;
        playerHealthDebugText.text = currentHealth.ToString();
        _knockOutTimer = playerKnockOutTimer;
        _reviveTimer = playerReviveTimer;
        playerstate = PlayerState.ALIVE;
        
        // Res part
        lifeSlider.maxValue = playerKnockOutTimer;
        lifeSlider.value = playerKnockOutTimer;
        knockOutCanvas.SetActive(false);
        
        StartCoroutine(ReviveInvincibility(invincibleDuration));
        
        // TODO - Enlever debug
        playerStateDebugText.text = $"ALIVE";
        playerStateDebugText.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerstate)
        {
            case PlayerState.ALIVE: break;
            case PlayerState.KNOCKOUT: OnKnockOut(); break;
            case PlayerState.DEAD: break;
            case PlayerState.REVIVE: OnRevive(); break;
            /*default: throw new ArgumentOutOfRangeException();*/
        }
    }

    #endregion

    #region Methods

    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        playerHealthDebugText.text = currentHealth.ToString();
        if (currentHealth > 0) return;

        // No more hp
        PlayerKnockOut();
    }

    private void PlayerKnockOut()
    {
        playerstate = PlayerState.KNOCKOUT;
        knockOutCanvas.SetActive(true);
        Debug.Log(knockOutCanvas);
        
        // TODO - Enlever debug
        playerStateDebugText.text = $"KNOCK-OUT";
        playerStateDebugText.color = Color.yellow;
    }

    private void PlayerDeath()
    {
        playerstate = PlayerState.DEAD;
        _knockOutTimer = playerKnockOutTimer;
        _reviveTimer = playerReviveTimer;
        lifeSlider.maxValue = playerKnockOutTimer;
        lifeSlider.value = playerKnockOutTimer;
        knockOutCanvas.SetActive(false);
        
        // TODO - Enlever debug
        playerStateDebugText.text = $"DEAD";
        playerStateDebugText.color = Color.black;
    }
    
    private void RevivePlayer()
    {
        InitPlayerHealth();
    }

    private void OnKnockOut()
    {
        _knockOutTimer -= Time.deltaTime;
        
        lifeSlider.value = _knockOutTimer;
        sliderImage.color = Color.Lerp(Color.red, Color.green, _knockOutTimer/playerKnockOutTimer);
        
        if (_knockOutTimer > 0) return;
        
        // No more time to Revive
        PlayerDeath();
    }

    private void OnRevive()
    {
        if (inReanimation)
        {
            playerReviveTimer += Time.deltaTime;
        }
        else
        {
            playerReviveTimer = 0;
        }
        
        inReanimation = true;
        
        if (_reviveTimer < playerReviveTimer) return;
        inReanimation = false;
        RevivePlayer();
    }

    
    [ContextMenu("Kill Player")]
    void Kill()
    {
        TakeDamage(99999);
    }

    IEnumerator ReviveInvincibility(float _invincibleDuration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(_invincibleDuration);
        isInvincible = false;
    }
    #endregion

    #region IInteractable Implementation
    public void Interact(PlayerInteraction _playerInteraction)
    {
        RevivePlayer(); // TODO Modif par rester appuyez 
        Debug.Log("Interact for revive");
        playerstate = PlayerState.REVIVE;
    }
 
    public float GetInteractionDuration()
    {
        return 3.5f;
    }

    public bool IsInteractable()
    {
        return playerstate == PlayerState.KNOCKOUT;
    }
    #endregion
}

public enum PlayerState
{
    ALIVE,
    KNOCKOUT,
    DEAD,
    REVIVE
}