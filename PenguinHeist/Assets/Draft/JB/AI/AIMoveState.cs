using System;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveState : AIState
{
    [Tooltip("Waypoints of the AI route")]
    public Vector3[] wayPoints;
    public Awareness awareness;
    private int currentWayPoint;

    public override void MoveTo(NavMeshAgent agent, Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public override AIState RunCurrentState(AIStateManager stateManager)
    {
        if (awareness.visibleTargets.Count > 0)
        {
            stateManager.player = awareness.visibleTargets[0];
            stateManager.OnPlayerDetected?.Invoke();
            //print("detected");
            return nextState;
        }
        if (stateManager.agent.remainingDistance <= stateManager.agent.stoppingDistance)
        {
            NextWayPoint(stateManager.agent);
        }
        return null;
    }

    public void NextWayPoint(NavMeshAgent agent)
    {
        currentWayPoint++;
        if (currentWayPoint >= wayPoints.Length)
        {
            currentWayPoint = 0;
        }
        MoveTo(agent, wayPoints[currentWayPoint]);
    }
}
