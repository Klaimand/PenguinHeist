using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveState : AIState
{
    public List<Vector3> wayPoints;
    [SerializeField] Awareness awareness;
    [SerializeField] AIChaseState chaseState;
    
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

        if (awareness.visibleTargets.Count > 0)
        {
            chaseState.player = awareness.visibleTargets[0];
            return chaseState;
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveTo(wayPoints[Random.Range(0, wayPoints.Count)]);
        }
        return null;
    }
}
