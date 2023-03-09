using UnityEngine;
using UnityEngine.AI;

public class MafiaAgentAttackState : AIAttackState
{
    public override AIState RunCurrentState(NavMeshAgent agent, Transform player, WeaponData weaponData, float attackRange,
        float moveBackRange, LayerMask obstacleMask)
    {
        if (Vector3.Distance(player.position , transform.position) < moveBackRange)
        {
            return nextState;
        }
        return base.RunCurrentState(agent, player, weaponData, attackRange, moveBackRange, obstacleMask);
    }
}
