using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CustomizationItemData", menuName = "ScriptableObjects/Customization Item Data", order = 1)]
public class CustomizationItemData : ScriptableEvent
{
    public CustomizationType customizationType;
    public Sprite[] customizationItemsImage;
    public string[] customizationItemsName;
}
