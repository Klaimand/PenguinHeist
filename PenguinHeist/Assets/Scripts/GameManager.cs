using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] EventsManager eventsManager;
    [SerializeField] KLD_AudioManager audioManager;
    public EventsManager EventsManager => eventsManager;
    public KLD_AudioManager AudioManager => audioManager;

    public GameObject playerPrefab;
    public PlayerController2[] playerList;

    #region Singleton instance

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    #endregion
}
