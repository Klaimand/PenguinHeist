using System;
using TMPro;
using UnityEngine;

public enum CustomizationType
{
    Hat,
    Glasses,
    Mustache,
    Neck,
    Flower
}

public class CustomizationMenuManager : MonoBehaviour
{
    public static CustomizationMenuManager instance;
    
    public PlayerCustomizationData player1CustomizationData;
    public PlayerCustomizationData player2CustomizationData;
    [SerializeField] private PlayerCustomization player1Customization;
    [SerializeField] private PlayerCustomization player2Customization;
    [SerializeField] Color[] colors;
    int player1ColorIndex = 0;
    int player2ColorIndex = 0;
    [Header("Debug")] 
    [SerializeField] private TextMeshProUGUI player1Text;
    [SerializeField] private TextMeshProUGUI player2Text;
    [SerializeField] private TextMeshProUGUI player1ConfirmText;
    [SerializeField] private TextMeshProUGUI player2ConfirmText;
    

    private bool player1Confirm;
    private bool player2Confirm;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        ChangeColor();
        Confirm();
    }

    void Init()
    {
        if (player1CustomizationData.color != default)
        {
            player1ColorIndex = Array.IndexOf(colors, player1CustomizationData.color);
        }

        if (player2CustomizationData.color != default)
        {
            player2ColorIndex = Array.IndexOf(colors, player2CustomizationData.color);
        }
    }

    public void SetCustomization(CustomizationType customizationType, string name, int player)
    {
        PlayerCustomizationData playerCustomizationData = player == 1 ? player1CustomizationData : player2CustomizationData;
        PlayerCustomization playerCustomization = player == 1 ? player1Customization : player2Customization;

        switch (customizationType)
        {
            case CustomizationType.Hat :
                playerCustomizationData.hat = name;
                break;
            case CustomizationType.Glasses :
                playerCustomizationData.glasses = name;
                break;
            case CustomizationType.Mustache :
                playerCustomizationData.mustache = name;
                break;
            case CustomizationType.Neck :
                playerCustomizationData.neck = name;
                break;
            case CustomizationType.Flower :
                playerCustomizationData.flower = name;
                break;
        }
        
        playerCustomization.SetCustomization(customizationType, name);
    }

    void ChangeColor()
    {
        if (Input.GetButtonDown("Change Color"))
        {
            player1ColorIndex++;
            if (player1ColorIndex >= colors.Length)
            {
                player1ColorIndex = 0;
            }
            player1CustomizationData.color = colors[player1ColorIndex];
            player1Customization.ChangeColor(player1CustomizationData.color);
        }
        else if (Input.GetButtonDown("Change ColorP2"))
        {
            player2ColorIndex++;
            if (player2ColorIndex >= colors.Length)
            {
                player2ColorIndex = 0;
            }
            player2CustomizationData.color = colors[player2ColorIndex];
            player2Customization.ChangeColor(player2CustomizationData.color);
        }
        
        //Debug
        player1Text.color = player1CustomizationData.color;
        player2Text.color = player2CustomizationData.color;
    }

    void Confirm()
    {
        if (Input.GetButtonDown("Confirm"))
        {
            player1Confirm = !player1Confirm;
            if (player1Confirm)
            {
                player1ConfirmText.text = "Confirmed";
            }
            else
            {
                player1ConfirmText.text = "Confirm ?";
            }
        }
        else if (Input.GetButtonDown("ConfirmP2"))
        {
            player2Confirm = !player2Confirm;
            if (player2Confirm)
            {
                player2ConfirmText.text = "Confirmed";
            }
            else
            {
                player2ConfirmText.text = "Confirm ?";
            }
        }
    }
}
