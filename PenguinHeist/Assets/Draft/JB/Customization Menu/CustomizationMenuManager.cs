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
    [Header("Debug")] [SerializeField] private TextMeshProUGUI player1Text;
    [SerializeField] private TextMeshProUGUI player2Text;

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
    }

    void Init()
    {
        if (player1CustomizationData.color != default)
        {
            //player1Customization.ChangeColor(player1CustomizationData.color);
            player1ColorIndex = Array.IndexOf(colors, player1CustomizationData.color);
        }

        if (player2CustomizationData.color != default)
        {
            //player2Customization.ChangeColor(player2CustomizationData.color);
            player2ColorIndex = Array.IndexOf(colors, player2CustomizationData.color);
        }
    }

    public void SetCustomization(CustomizationType customizationType, GameObject gameObject, int player)
    {
        PlayerCustomizationData playerCustomizationData = player == 1 ? player1CustomizationData : player2CustomizationData;
        PlayerCustomization playerCustomization = player == 1 ? player1Customization : player2Customization;

        switch (customizationType)
        {
            case CustomizationType.Hat :
                playerCustomizationData.hat = gameObject;
                return;
            case CustomizationType.Glasses :
                playerCustomizationData.glasses = gameObject;
                return;
            case CustomizationType.Mustache :
                playerCustomizationData.mustache = gameObject;
                return;
            case CustomizationType.Neck :
                playerCustomizationData.neck = gameObject;
                return;
            case CustomizationType.Flower :
                playerCustomizationData.flower = gameObject;
                return;
        }
        
        //playerCustomization.SetCustomization(customizationType, gameObject);
    }

    public void ChangeColor()
    {
        if (Input.GetButtonDown("Change Color"))
        {
            player1ColorIndex++;
            if (player1ColorIndex >= colors.Length)
            {
                player1ColorIndex = 0;
            }
            player1CustomizationData.color = colors[player1ColorIndex];
            //player1Customization.ChangeColor(player1CustomizationData.color);
        }
        else if (Input.GetButtonDown("Change ColorP2"))
        {
            player2ColorIndex++;
            if (player2ColorIndex >= colors.Length)
            {
                player2ColorIndex = 0;
            }
            player2CustomizationData.color = colors[player2ColorIndex];
            //player2Customization.ChangeColor(player2CustomizationData.color);
        }
        
        //Debug
        player1Text.color = player1CustomizationData.color;
        player2Text.color = player2CustomizationData.color;
    }
}
