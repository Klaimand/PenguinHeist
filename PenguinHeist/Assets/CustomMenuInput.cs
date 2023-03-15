using UnityEngine;


public class CustomMenuInput : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private string changeColorInput;
    [SerializeField] private string confirmInput;

    private void Start()
    {
        switch (playerIndex)
        {
            case 0: changeColorInput = $"Change Color"; confirmInput = $"Confirm"; break;
            case 1: changeColorInput = $"Change ColorP2"; confirmInput = $"ConfirmP2"; break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(CustomizationMenuManager.instance.isTwoPlayerReady) return;
        if (Input.GetButtonDown(changeColorInput)) ColorInput();
        if (Input.GetButtonDown(confirmInput)) ConfirmInput();
    }

    void ConfirmInput()
    {
        CustomizationMenuManager.instance.Confirm(playerIndex);
    }

    void ColorInput()
    {
        CustomizationMenuManager.instance.ChangeColor(playerIndex);
    }
}