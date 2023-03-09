using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIChaseState : AIState
{
    public override void MoveTo(NavMeshAgent agent, Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public override AIState RunCurrentState(NavMeshAgent agent, Transform player, WeaponData weaponData, float attackRange, float moveBackRange, LayerMask obstacleMask)
    {
        MoveTo(agent, player.position);
        if (agent.remainingDistance <= weaponData.range)
        {
            return nextState;
        }
        return null;
    }
}
