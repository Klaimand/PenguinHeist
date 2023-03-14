using UnityEngine;
using UnityEngine.AI;

public class AIChaseAndAttackState : AIAttackState
{

    public override void MoveTo(NavMeshAgent agent, Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public override AIState RunCurrentState(AIStateManager stateManager)
    {
        MoveTo(stateManager.agent, stateManager.player.position);
        if (!Physics.Raycast(transform.position, stateManager.player.position - transform.position, out hit, Vector3.Distance(transform.position, stateManager.player.position), stateManager.obstacleMask))
        {
            CheckAttack(stateManager.weaponData, stateManager.entity);
        }

        if (stateManager.agent.remainingDistance > stateManager.chaseAndAttackRange)
        {
            return previousState;
        }

        if (stateManager.agent.remainingDistance < stateManager.attackRange)
        {
            if (!Physics.Raycast(transform.position, stateManager.player.position - transform.position, out hit, Vector3.Distance(transform.position, stateManager.player.position), stateManager.obstacleMask))
            {
                stateManager.agent.Stop();
                stateManager.agent.avoidancePriority = 2;
                return nextState;
            }
        }
        return null;
    }
}
