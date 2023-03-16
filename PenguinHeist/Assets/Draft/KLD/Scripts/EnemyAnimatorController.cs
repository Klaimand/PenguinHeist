using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimatorController : MonoBehaviour
{

    [SerializeField] Animator animator;

    [SerializeField] AIStateManager aIStateManager;
    [SerializeField] AIEntity aIEntity;
    [SerializeField] AIAttackState aIAttackState;
    [SerializeField] Transform gunParent;
    [SerializeField] AIMoveState aIMoveState;

    [SerializeField] UnityEvent onPlayerDetected;

    void OnEnable()
    {
        aIAttackState.OnEnemyShoot += Shoot;
        aIEntity.OnHit += Hit;

        aIStateManager.OnPlayerDetected += OnPlayerDetected;
    }

    void OnDisable()
    {
        aIAttackState.OnEnemyShoot -= Shoot;
        aIEntity.OnHit -= Hit;

        aIStateManager.OnPlayerDetected -= OnPlayerDetected;
    }

    void Start()
    {
        gunParent.GetChild(aIEntity.Weapon.weaponIndex).gameObject.SetActive(true);
    }

    void Update()
    {
        animator.SetInteger("enemyState", (int)aIStateManager.aIStateType);
        animator.SetBool("isReloading", aIEntity.isReloading);
        animator.SetBool("isShielded", aIEntity.isHoldingShield);
    }

    void Shoot()
    {
        animator.SetTrigger("shoot");
    }

    void Hit()
    {
        animator.SetTrigger("hit");
    }

    void OnPlayerDetected()
    {
        onPlayerDetected.Invoke();
    }
}
