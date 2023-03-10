using UnityEngine;
using UnityEngine.AI;

public class AIStateManager : MonoBehaviour
{
    [Header("State")]
    [SerializeField] public AIState currentState;
    [Header("NavMesh")]
    public NavMeshAgent agent;
    [Header("Data")]
    public WeaponData weaponData;
    public float attackRange;
    public float moveBackRange;
    [SerializeField] Transform player;
    [SerializeField] LayerMask obstacleMask;
    
    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        AIState nextState = currentState?.RunCurrentState(agent, player, weaponData, attackRange, moveBackRange, obstacleMask);

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
