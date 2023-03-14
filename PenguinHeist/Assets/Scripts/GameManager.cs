using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] EventsManager eventsManager;
    public EventsManager EventsManager => eventsManager;

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
    
    private void Start()
    {
        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].playerIndex = i;
        }
    }
    
}
