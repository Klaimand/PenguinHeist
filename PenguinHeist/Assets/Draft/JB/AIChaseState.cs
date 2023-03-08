using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIChaseState : AIState
{
    public  WeaponData weaponData;
    [SerializeField] AIAttackState attackState;
    [SerializeField] LayerMask obstacleMask;
    public Transform player;
    
    public override void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public override AIState RunCurrentState(NavMeshAgent agent)
    {
        if (agent != default)
        {
            this.agent = agent;
        }
        if (agent.remainingDistance <= weaponData.range)
        {
            if (!Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hit, weaponData.range, obstacleMask))
            {
                attackState.player = player;
                agent.Stop();
                agent.destination = player.position;
                agent.avoidancePriority = 2;
                return attackState;
            }
        }
        MoveTo(player.position);
        return null;
    }
}
