using UnityEngine;
using UnityEngine.AI;

public class AITakeBagState : AIState
{
    private Transform bag;
    
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
        MoveTo(stateManager.agent, stateManager.player.position);
        if (stateManager.agent.remainingDistance <= stateManager.chaseAndAttackRange)
        {
            return nextState;
        }
        return null;
    }
}
