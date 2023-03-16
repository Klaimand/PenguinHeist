using UnityEngine;

public class Shield : MonoBehaviour, IDamageable
{
    [SerializeField] int health = 100;
    [SerializeField] AIEntity aiEntity;
    [SerializeField] ParticleSystem shieldImpact;

    bool dead = false;

    public void TakeDamage(int _damage)
    {
        health -= _damage;
        shieldImpact.Play();
        if (!dead && health <= 0)
        {
            dead = true;
            aiEntity.isHoldingShield = false;

            //gameObject.layer = LayerMask.NameToLayer("Default");
            transform.parent = null;
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.AddTorque(Random.onUnitSphere * 20f);
            Destroy(gameObject, 10f);
        }
    }
}
