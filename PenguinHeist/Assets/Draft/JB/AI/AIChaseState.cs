using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIChaseState : AIState
{
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
