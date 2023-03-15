using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{

    [SerializeField] Animator animator;

    [SerializeField] AIStateManager aIStateManager;
    [SerializeField] AIEntity aIEntity;
    [SerializeField] AIAttackState aIAttackState;
    [SerializeField] Transform gunParent;

    void OnEnable()
    {
        aIAttackState.OnEnemyShoot += Shoot;
        aIEntity.OnHit += Hit;
    }

    void OnDisable()
    {
        aIAttackState.OnEnemyShoot -= Shoot;
        aIEntity.OnHit -= Hit;
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
}
