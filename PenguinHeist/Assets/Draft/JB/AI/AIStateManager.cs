using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateManager : MonoBehaviour
{
    [Header("State")]
    [SerializeField] public AIState currentState;
    [Header("NavMesh")]
    public NavMeshAgent agent;
    [Header("Data")]
    [HideInInspector] public WeaponSO weaponData;
    public float chaseAndAttackRange;
    public float attackRange;
    public float moveBackRange;
    public Transform player;
    public LayerMask obstacleMask;
    public AIEntity entity;

    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        AIState nextState = currentState?.RunCurrentState(this);

        if (nextState != default)
        {
            SwitchToTheNextState(nextState);
        }
    }

    private void SwitchToTheNextState(AIState nextState)
    {
        currentState = nextState;
    }
}
