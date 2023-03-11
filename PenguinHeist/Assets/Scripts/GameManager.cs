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

    

    private string[] joystickNames;

    private void Start()
    {
        joystickNames = Input.GetJoystickNames();

        foreach (string joystickName in joystickNames)
        {
            Debug.Log(joystickName);
        }

        foreach (var t in joystickNames)
        {
            Debug.Log(t.Length);
            if (t.Length == 33) Debug.Log("XBOX");
        }
        
        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].playerIndex = i;
        }
    }
    
}
