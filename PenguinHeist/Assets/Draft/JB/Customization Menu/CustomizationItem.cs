using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizationItem : MonoBehaviour, IUpdateSelectedHandler
{
    [SerializeField] CustomizationItemData customizationItemData;
    [SerializeField] private int player = 1;
    [SerializeField] private float nextItemDelay = 0.3f;
    private Image image;
    private float currentDelay;
    int currentIndex = 0;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        currentDelay -= Time.deltaTime;
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        NextCustomization();
    }

    void Init()
    {
        PlayerCustomizationData playerCustomizationData = player == 1 ? CustomizationMenuManager.instance.player1CustomizationData : CustomizationMenuManager.instance.player2CustomizationData;
        CustomizationType customizationType = customizationItemData.customizationType;
        switch (customizationType)
        {
            case CustomizationType.Hat :
                if (playerCustomizationData.hat == String.Empty)
                {
                    return;
                }
                currentIndex = Array.IndexOf(customizationItemData.customizationItemsName, playerCustomizationData.hat);
                break;
            case CustomizationType.Glasses :
                if (playerCustomizationData.glasses == String.Empty)
                {
                    return;
                }
                currentIndex = Array.IndexOf(customizationItemData.customizationItemsName, playerCustomizationData.glasses);
                break;
            case CustomizationType.Mustache :
                if (playerCustomizationData.mustache == String.Empty)
                {
                    return;
                }
                currentIndex = Array.IndexOf(customizationItemData.customizationItemsName, playerCustomizationData.mustache);
                break;
            case CustomizationType.Neck :
                if (playerCustomizationData.neck == String.Empty)
                {
                    return;
                }
                currentIndex = Array.IndexOf(customizationItemData.customizationItemsName, playerCustomizationData.neck);
                break;
            case CustomizationType.Flower :
                if (playerCustomizationData.flower == String.Empty)
                {
                    return;
                }
                currentIndex = Array.IndexOf(customizationItemData.customizationItemsName, playerCustomizationData.flower);
                break;
        }
        image.sprite = customizationItemData.customizationItemsImage[currentIndex];
    }
    
    public void NextCustomization()
    {
        string playerNb = player == 1 ? String.Empty : "P2";
        if (Mathf.Abs(Input.GetAxis("Vertical" + playerNb)) >= 0.99f && currentDelay < 0)
        {
            if (Input.GetAxis("Vertical" + playerNb) >= 0.99f)
            {
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = customizationItemData.customizationItemsImage.Length - 1;
                }
            }
            else if (Input.GetAxis("Vertical" + playerNb) <= -0.99f)
            {
                currentIndex++;
                if (currentIndex > customizationItemData.customizationItemsImage.Length - 1)
                {
                    currentIndex = 0;
                }
            }
            image.sprite = customizationItemData.customizationItemsImage[currentIndex];
            CustomizationMenuManager.instance.SetCustomization(customizationItemData.customizationType, customizationItemData.customizationItemsName[currentIndex], player);
            currentDelay = nextItemDelay;
        }
    }


}
