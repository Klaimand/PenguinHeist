using UnityEngine;
using UnityEngine.AI;

public class AIChaseAndAttackState : AIState
{
    [SerializeField] WeaponData weaponData;
    public Transform player;

    
    private void Start()
    {
        stateManager = GetComponent<AIStateManager>(); ;
    }

    public override void MoveTo(Vector3 destination)
    {
        stateManager.agent.SetDestination(destination);
    }

    public override AIState RunCurrentState(NavMeshAgent agent)
    {
        if (agent != default)
        {
            stateManager.agent = agent;
        }
        if (agent.remainingDistance <= weaponData.range)
        {
            if (!Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hit, weaponData.range))
            {
                agent.Stop();
                agent.destination = player.position;
                agent.avoidancePriority = 2;
                return nextState;
            }
        }
        MoveTo(player.position);
        return null;
    }
}
