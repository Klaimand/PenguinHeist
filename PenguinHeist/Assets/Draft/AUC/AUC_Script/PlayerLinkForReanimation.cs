using UnityEngine;

public class PlayerLinkForReanimation : MonoBehaviour
{
    public PlayerController2 myController;
    public PlayerController2 otherPlayer;
    public bool canRez;
    public GameObject reanimationCanvas;

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (other.gameObject.GetComponent<PlayerController2>() !=  myController)
        {
            Debug.Log("Play Everyframe?");
            otherPlayer = other.gameObject.GetComponent<PlayerController2>();
        }
        UpdateReanimationState();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        Debug.Log("Play Everyframe?");
        otherPlayer = null;
        
        UpdateReanimationState();
    }
    
    void UpdateReanimationState()
    {
        canRez = otherPlayer != null;
        reanimationCanvas.SetActive(canRez);
    }
}
