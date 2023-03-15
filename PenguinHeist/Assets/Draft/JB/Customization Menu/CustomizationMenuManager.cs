using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

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
    
    public PlayerCustomizationData player1CustomizationData = default;
    public PlayerCustomizationData player2CustomizationData = default;
    
    [SerializeField] private PlayerCustomization player1Customization;
    [SerializeField] private PlayerCustomization player2Customization;
    
    [SerializeField] Color[] colors;
    
    int player1ColorIndex = 0;
    int player2ColorIndex = 0;
    
    [Header("Buttons")]
    [SerializeField] private RectTransform player1ChangeColorButton;
    [SerializeField] private RectTransform player2ChangeColorButton;
    [SerializeField] private RectTransform player1ConfirmButton;
    [SerializeField] private RectTransform player2ConfirmButton;
    
    [Header("Confirm Texts")]
    [SerializeField] private TextMeshProUGUI player1ConfirmText;
    [SerializeField] private TextMeshProUGUI player2ConfirmText;

    [Header("Players Ready")] 
    [SerializeField] private Image playersReadyBackGround;
    [SerializeField] private RectTransform readyText;
    [SerializeField] private float launchDelay = 3;
    [SerializeField] private TextMeshProUGUI launchText;
    
    private bool isPlayer1Confirmed;

    private bool isPlayer2Confirmed;
    
    [HideInInspector] public bool isTwoPlayerReady;

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

    private void Update()
    {
        if (isTwoPlayerReady)
        {
            launchDelay -= Time.deltaTime;
            launchText.text = Mathf.CeilToInt(launchDelay).ToString();
            if (launchDelay <= 0)
            {
                //Launch Game
            }
        }
    }

    private void Start()
    {
        Init();
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

   public void ChangeColor(int playerIndex)
    {
        if (playerIndex == 0 )
        {
            player1ColorIndex++;
            
            if (player1ColorIndex >= colors.Length) player1ColorIndex = 0;
            
            player1CustomizationData.color = colors[player1ColorIndex];
            player1Customization.ChangeColor(player1CustomizationData.color);
            
            UIAnimation.DoPUnchScale(player1ChangeColorButton, 0.1f, 0.2f);
        }
        else
        {
            player2ColorIndex++;
            if (player2ColorIndex >= colors.Length) player2ColorIndex = 0;
            
            player2CustomizationData.color = colors[player2ColorIndex];
            player2Customization.ChangeColor(player2CustomizationData.color);
            
            UIAnimation.DoPUnchScale(player2ChangeColorButton, 0.1f, 0.2f);
        }
    }

    public void Confirm(int playerIndex)
    {
        CustomizationAnimationManager.instance.TriggerLever(playerIndex);
        
        if (playerIndex == 0)
        {
            isPlayer1Confirmed = !isPlayer1Confirmed;
            player1ConfirmText.text = isPlayer1Confirmed ? "Confirmed" : "Confirm ?";
            UIAnimation.DoPUnchScale(player1ConfirmButton, 0.1f, 0.2f);
        }
        else
        {
            isPlayer2Confirmed = !isPlayer2Confirmed;
            player2ConfirmText.text = isPlayer2Confirmed ? "Confirmed" : "Confirm ?";
            UIAnimation.DoPUnchScale(player2ConfirmButton, 0.1f, 0.2f);
        }
        CheckIfReady();
    }
    
    void CheckIfReady()
    {
        if (isPlayer1Confirmed && isPlayer2Confirmed)
        {
            isTwoPlayerReady = true;
            UIAnimation.DoFade(playersReadyBackGround, 1, 186/255f);
            UIAnimation.DoMove(readyText, 2, new Vector2(Screen.width/2, Screen.height/2));
            launchText.gameObject.SetActive(true);
        }
    }
}
