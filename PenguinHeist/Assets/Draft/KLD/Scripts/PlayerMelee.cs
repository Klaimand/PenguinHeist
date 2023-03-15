using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{

    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerBag playerBag;
    [SerializeField] PlayerAnimationController animationController;

    [SerializeField] float meleeAttackDuration = 0.49f;

    bool isAttacking = false;
    public bool IsAttacking => isAttacking;
    
    [Header("Input")]
    public string meleeInput;
    public float lTrigger;
    public bool isPressingMeleeInput;
    
    [Header("Melee Contact Point")]
    [SerializeField] private Transform meleeContactPoint;
    [SerializeField] private float meleeContactRadius = 0.5f;
    [SerializeField] private int damage = 10;

    private void Start()
    {
        var playerIndex = GetComponent<PlayerController2>().playerIndex;
        meleeInput = playerIndex switch
        {
            0 => $"LeftTrigger",
            1 => $"LeftTriggerP2",
            _ => meleeInput
        };
    }

    void Update()
    {
        CheckMelee();
    }

    void CheckMelee()
    {
        if (playerInteraction.IsInteracting) return;
        if (playerBag.IsCarrying) return;
        //if player is dead return
        if (isAttacking) return;
        
        lTrigger = Input.GetAxis(meleeInput);
        isPressingMeleeInput = lTrigger > 0.5f;
        
        if (isPressingMeleeInput)
        {
            animationController.Melee();
            isAttacking = true;
            StartCoroutine(WaitAndCanAttack());
        }
    }

    IEnumerator WaitAndCanAttack()
    {
        StartCoroutine(MakeDamage());
        yield return new WaitForSeconds(meleeAttackDuration);
        isAttacking = false;
    }
    
    IEnumerator MakeDamage()
    {
        yield return new WaitForSeconds(meleeAttackDuration - 0.1f);
        Collider[] hitColliders = Physics.OverlapSphere(meleeContactPoint.position, meleeContactRadius);
        IDamageable damageable;
        foreach (var hitCollider in hitColliders)
        {
            damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != default)
            {
                hitCollider.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (meleeContactPoint != default)
        {
            Gizmos.DrawWireSphere(meleeContactPoint.position, meleeContactRadius);
        }
    }
}
