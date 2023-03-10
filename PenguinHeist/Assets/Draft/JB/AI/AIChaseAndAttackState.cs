using UnityEngine;
using UnityEngine.AI;

public class AIChaseAndAttackState : AIAttackState
{

    private void Update()
    {
        currentAttackCd -= Time.deltaTime;
    }
    
    public override void MoveTo(NavMeshAgent agent, Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public override AIState RunCurrentState(NavMeshAgent agent, Transform player, WeaponData weaponData, float attackRange, float moveBackRange, LayerMask obstacleMask)
    {
        MoveTo(agent, player.position);
        if (!Physics.Raycast(transform.position, player.position - transform.position, out hit, Vector3.Distance(transform.position, player.position), obstacleMask))
        {
            Attack(weaponData);
        }
        
        if (agent.remainingDistance > weaponData.range)
        {
            return previousState;
        }

        if (agent.remainingDistance < attackRange)
        {
            if (!Physics.Raycast(transform.position, player.position - transform.position, out hit, Vector3.Distance(transform.position, player.position), obstacleMask))
            {
                agent.Stop();
                agent.avoidancePriority = 2;
                return nextState;
            }
        }
        return null;
    }
}
