using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] float speed = 15f;
    [SerializeField] int damage = 10;

    [SerializeField] GameObject impactFX = null;

    bool collided = false;

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

        Instantiate(impactFX, transform.position, Quaternion.LookRotation(other.GetContact(0).normal));

        Destroy(gameObject, 1f);
    }

}
