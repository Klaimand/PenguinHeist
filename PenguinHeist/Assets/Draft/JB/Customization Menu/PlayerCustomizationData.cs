using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCustomizationData", menuName = "ScriptableObjects/Player Customization Data", order = 1)]
public class PlayerCustomizationData : ScriptableObject
{
    public GameObject hat;
    public GameObject glasses;
    public GameObject mustache;
    public GameObject neck;
    public GameObject flower;
    public Color color;
}
