using UnityEngine;
using UnityEngine.AI;

public enum AIType
{
    Police,
    Mafia
}

public class AIStateManager : MonoBehaviour
{
    [Header("State")]
    [SerializeField] public AIState currentState;
    [Header("NavMesh")]
    public NavMeshAgent agent;
    public AIType aiType;
    public WeaponData weaponData;
    public float attackRange;
    public float moveBackRange;
    public Transform player;
    
    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        AIState nextState = currentState?.RunCurrentState(agent);

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
