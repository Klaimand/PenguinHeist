using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIChaseState : AIState
{
    [SerializeField] AIAttackState attackState;
    [SerializeField] LayerMask obstacleMask;
    
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
        if (agent.remainingDistance <= stateManager.weaponData.range)
        {
            if (!Physics.Raycast(transform.position, stateManager.player.position - transform.position, out RaycastHit hit, stateManager.weaponData.range, obstacleMask))
            {
                agent.Stop();
                agent.destination = stateManager.player.position;
                agent.avoidancePriority = 2;
                return nextState;
            }
        }
        MoveTo(stateManager.player.position);
        return null;
    }
}
