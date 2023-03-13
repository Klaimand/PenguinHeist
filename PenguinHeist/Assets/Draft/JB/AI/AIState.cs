using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : MonoBehaviour
{
    [Tooltip("Next state to run when this state is finished")]
    [SerializeField] public AIState nextState;
    [Tooltip("Previous state to run when this state is finished")]
    [SerializeField] public AIState previousState;
    public AIStateType aIStateType;
    public abstract void MoveTo(NavMeshAgent agent, Vector3 destination);
    public abstract AIState RunCurrentState(AIStateManager stateManager);
}
