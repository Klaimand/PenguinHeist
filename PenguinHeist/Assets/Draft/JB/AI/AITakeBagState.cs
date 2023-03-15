using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class AITakeBagState : AIState
{
    [HideInInspector] public Transform bag;
    
    public void Init(NavMeshAgent agent, Transform bag)
    {
        this.bag = bag;
        MoveTo(agent, bag.position);
    }
    
    public override void MoveTo(NavMeshAgent agent, Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public override AIState RunCurrentState(AIStateManager stateManager)
    {
        MoveTo(stateManager.agent, new Vector3(bag.position.x, 0.5f, bag.position.z));
        if (stateManager.agent.remainingDistance <= stateManager.agent.stoppingDistance && Vector3.Distance(stateManager.transform.position, new Vector3(bag.position.x, 0.5f, bag.position.z)) <= stateManager.agent.stoppingDistance)
        {
            stateManager.aIStateType = AIStateType.Interact;
        }
        return null;
    }
}
