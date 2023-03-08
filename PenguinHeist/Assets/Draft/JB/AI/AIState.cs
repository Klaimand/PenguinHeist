using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : MonoBehaviour
{
    [SerializeField] public AIState nextState;
    [SerializeField] public AIState previousState;
    public AIStateManager stateManager;
    public abstract void MoveTo(Vector3 destination);
    public abstract AIState RunCurrentState(NavMeshAgent agent);
}
