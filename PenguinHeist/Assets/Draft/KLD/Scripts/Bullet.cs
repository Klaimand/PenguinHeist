using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] float speed = 15f;
    [SerializeField] int damage = 10;

    private void Start()
    {

    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage);
        print(other.gameObject.name);
        print(transform.position.y);
        Destroy(gameObject);
    }

}
