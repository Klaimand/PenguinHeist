using System;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveState : AIState
{
    public Vector3[] wayPoints;
    public Awareness awareness;
    private int currentWayPoint;

    public override void MoveTo(NavMeshAgent agent, Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public override AIState RunCurrentState(NavMeshAgent agent, Transform player, WeaponData weaponData, float attackRange, float moveBackRange, LayerMask obstacleMask)
    {
        if (awareness.visibleTargets.Count > 0)
        {
            return nextState;
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            NextWayPoint(agent);
        }
        return null;
    }

    public void NextWayPoint(NavMeshAgent agent)
    {
        currentWayPoint++;
        if (currentWayPoint>=wayPoints.Length)
        {
            currentWayPoint = 0;
        }
        MoveTo(agent, wayPoints[currentWayPoint]);
    }
}
