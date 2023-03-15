using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCustomization : MonoBehaviour
{
    [SerializeField] PlayerCustomizationData playerCustomizationData;
    [HideInInspector] public string hat;
    [HideInInspector] public string glasses;
    [HideInInspector] public string mustache;
    [HideInInspector] public string neck;
    [HideInInspector] public string flower;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    
    GameObject hatGameObject;
    GameObject glassesGameObject;
    GameObject mustacheGameObject;
    GameObject neckGameObject;
    GameObject flowerGameObject;

    private void Start()
    {
        SetCustomization(CustomizationType.Hat, playerCustomizationData.hat);
        SetCustomization(CustomizationType.Glasses, playerCustomizationData.glasses);
        SetCustomization(CustomizationType.Mustache, playerCustomizationData.mustache);
        SetCustomization(CustomizationType.Neck, playerCustomizationData.neck);
        SetCustomization(CustomizationType.Flower, playerCustomizationData.flower);
        ChangeColor(playerCustomizationData.color);
    }

    public void SetCustomization(CustomizationType customizationType, string name)
    {
        switch (customizationType)
        {
            case CustomizationType.Hat :
                if (hat != String.Empty)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).name.Equals(hat))
                        {
                           transform.GetChild(i).gameObject.SetActive(false);
                           break;
                        }
                    }
                }
                SetItem(out hat, name);
                return;
            case CustomizationType.Glasses :
                if (glasses != String.Empty)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).name.Equals(glasses))
                        {
                            transform.GetChild(i).gameObject.SetActive(false); 
                            break;
                        }
                    }
                }
                SetItem(out glasses, name);
                return;
            case CustomizationType.Mustache :
                if (mustache != String.Empty)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).name.Equals(mustache))
                        {
                            transform.GetChild(i).gameObject.SetActive(false);
                            break;
                        }
                    }
                }
                SetItem(out mustache, name);
                return;
            case CustomizationType.Neck :
                if (neck != String.Empty)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).name.Equals(neck))
                        {
                            transform.GetChild(i).gameObject.SetActive(false); 
                            break;
                        }
                    }
                }
                SetItem(out neck, name);
                return;
            case CustomizationType.Flower :
                if (flower != String.Empty)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).name.Equals(flower))
                        {
                            transform.GetChild(i).gameObject.SetActive(false); 
                            break;
                        }
                    }
                }
                SetItem(out flower, name);
                return;
        }
    }

    public void SetItem(out string item, string name)
    {
        item = name;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Equals(name))
            {
                transform.GetChild(i).gameObject.SetActive(true); 
            }
        }
    }

    public void ChangeColor(Color color)
    {
        skinnedMeshRenderer.material.SetColor("_CustomColor", color);
    }
}
