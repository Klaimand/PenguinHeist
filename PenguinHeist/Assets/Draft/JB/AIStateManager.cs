using UnityEngine;
using UnityEngine.AI;

public class AIStateManager : MonoBehaviour
{
    [Header("State")]
    [SerializeField]  AIState currentState;
    [Header("NavMesh")]
    public NavMeshAgent agent;
    
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
