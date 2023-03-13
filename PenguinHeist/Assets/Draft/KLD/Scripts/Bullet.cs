using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] float speed = 15f;
    [SerializeField] int damage = 10;
    private float timeBeforeSelfDestroy = 10f;

    [SerializeField] GameObject impactFX = null;

    bool collided = false;

    private void Start()
    {
        Destroy(gameObject, timeBeforeSelfDestroy);
    }

    void FixedUpdate()
    {
        if (collided) return;

        rb.MovePosition(transform.position + (transform.forward * speed * Time.fixedDeltaTime));
    }

    void OnCollisionEnter(Collision other)
    {
        if (collided) return;

        collided = true;

        other.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage);

        Vector3 hitNormal = Vector3.Lerp(other.GetContact(0).normal, -transform.forward, 0.5f);

        Instantiate(impactFX, transform.position, Quaternion.LookRotation(hitNormal));

        //Destroy(gameObject, 1f);
        Destroy(gameObject);
    }

}
