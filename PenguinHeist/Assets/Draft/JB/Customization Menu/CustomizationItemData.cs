using UnityEngine;

[CreateAssetMenu(fileName = "CustomizationItemData", menuName = "ScriptableObjects/Customization Item Data", order = 1)]
public class CustomizationItemData : ScriptableEvent
{
    public CustomizationType customizationType;
    public Sprite[] customizationItemsImage;
    public GameObject[] customizationItemsGameObject;
}
