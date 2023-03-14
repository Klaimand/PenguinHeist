using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
    public Transform hat;
    public Transform glasses;
    public Transform mustache;
    public Transform neck;
    public Transform flower;
    [SerializeField] Material material;
    
    GameObject hatGameObject;
    GameObject glassesGameObject;
    GameObject mustacheGameObject;
    GameObject neckGameObject;
    GameObject flowerGameObject;

    public void SetCustomization(CustomizationType customizationType, GameObject gameObject)
    {
        switch (customizationType)
        {
            case CustomizationType.Hat :
                if (hat != null)
                {
                    Destroy(hatGameObject);
                }
                SetItem(hat, gameObject);
                return;
            case CustomizationType.Glasses :
                if (glasses != null)
                {
                    Destroy(glassesGameObject);
                }
                SetItem(glasses, gameObject);
                return;
            case CustomizationType.Mustache :
                if (mustache != null)
                {
                    Destroy(mustacheGameObject);
                }
                SetItem(mustache, gameObject);
                return;
            case CustomizationType.Neck :
                if (neck != null)
                {
                    Destroy(neckGameObject);
                }
                SetItem(neck, gameObject);
                return;
            case CustomizationType.Flower :
                if (flower != null)
                {
                    Destroy(flowerGameObject);
                }
                SetItem(flower, gameObject);
                return;
        }
    }

    public GameObject SetItem(Transform item, GameObject gameObject)
    {
        return Instantiate(gameObject, item.position, Quaternion.identity, item);
    }

    public void ChangeColor(Color color)
    {
        material.SetColor("_CustomColor", color);
    }
}
