using UnityEngine;

public class PlayerOpenChest : MonoBehaviour
{
    public string yInput;
    public Safe currentSafe;
    void Start()
    {
        var playerIndex = GetComponent<PlayerController2>().playerIndex;
        yInput = playerIndex switch
        {
            0 => $"Change Color",
            1 => $"Change ColorP2",
        };    
    }

    // Update is called once per frame
    void Update()
    {
        if(currentSafe == null) return;
        SmashInput();
    }

    private void SmashInput()
    {
        if (Input.GetButtonDown(yInput)) PressY();
    }

    private void PressY()
    {
        currentSafe.inputPressed = true;
    }
}
