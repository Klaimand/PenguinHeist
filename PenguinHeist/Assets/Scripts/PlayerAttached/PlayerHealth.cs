using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    #region Variables

    public int playerHealth = 100;
    public PlayerState playerstate;

    public float playerKnockOutTimer = 15f;
    private float _knockOutTimer = default;

    public float playerReviveTimer = 7.5f;
    private float _reviveTimer = default;

    public TextMeshProUGUI playerHealthDebugText;
    public TextMeshProUGUI playerStateDebugText;

    #endregion

    #region Event Methods

    // Start is called before the first frame update
    void Start()
    {
        playerHealthDebugText.text = playerHealth.ToString();
        playerstate = PlayerState.ALIVE;
        _knockOutTimer = playerKnockOutTimer;
        _reviveTimer = playerReviveTimer;
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
        playerHealth -= _damage;
        playerHealthDebugText.text = playerHealth.ToString();
        if (playerHealth > 0) return;

        // No more hp
        PlayerKnockOut();
    }

    private void PlayerKnockOut()
    {
        playerstate = PlayerState.KNOCKOUT;
        // TODO - Enlever debug
        playerStateDebugText.text = $"KNOCK-OUT";
        playerStateDebugText.color = Color.yellow;
    }

    private void PlayerDeath()
    {
        playerstate = PlayerState.DEAD;
        _knockOutTimer = playerKnockOutTimer;
        // TODO - Enlever debug
        playerStateDebugText.text = $"DEAD";
        playerStateDebugText.color = Color.black;
    }

    private void OnKnockOut()
    {
        _knockOutTimer -= Time.deltaTime;
        if (_knockOutTimer > 0) return;

        // No more time to Revive
        PlayerDeath();
    }

    private void OnRevive()
    {
        playerReviveTimer += Time.deltaTime;
    }

    [ContextMenu("Kill Player")]
    void Kill()
    {
        TakeDamage(99999);
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