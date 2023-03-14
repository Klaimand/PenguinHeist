using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{

    [SerializeField] Animator animator;

    [SerializeField] AIStateManager aIStateManager;
    [SerializeField] AIEntity aIEntity;
    [SerializeField] AIAttackState aIAttackState;


    void OnEnable()
    {
        aIAttackState.OnEnemyShoot += Shoot;
    }

    void OnDisable()
    {
        aIAttackState.OnEnemyShoot -= Shoot;
    }

    // Update is called once per frame
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
}
