using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{

    [SerializeField] Animator animator;

    [SerializeField] AIStateManager aIStateManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("enemyState", (int)aIStateManager.aIStateType);
    }
}
