using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    public abstract void MoveTo(Vector3 destination);
    public abstract AIState RunCurrentState(NavMeshAgent agent);
}
