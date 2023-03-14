using UnityEngine;

public class Shield : MonoBehaviour, IDamageable
{
    [SerializeField] int health = 100;
    [SerializeField] AIEntity aiEntity;
    
    public void TakeDamage(int _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            aiEntity.isHoldingShield = false;
            Destroy(gameObject);
        }
    }
}
