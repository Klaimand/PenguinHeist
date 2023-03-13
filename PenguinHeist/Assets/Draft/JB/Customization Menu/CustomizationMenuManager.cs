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
}
