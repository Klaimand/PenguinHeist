using System;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveState : AIState
{
    public Vector3[] wayPoints;
    [SerializeField] Awareness awareness;
    private int currentWayPoint;

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

        if (awareness.visibleTargets.Count > 0)
        {
            return nextState;
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            NextWayPoint();
        }
        return null;
    }

    public void NextWayPoint()
    {
        currentWayPoint++;
        if (currentWayPoint>=wayPoints.Length)
        {
            currentWayPoint = 0;
        }
        MoveTo(wayPoints[currentWayPoint]);
    }
}
