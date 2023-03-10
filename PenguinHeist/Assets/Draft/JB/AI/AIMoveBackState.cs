using UnityEngine;
using UnityEngine.AI;

public class AIMoveBackState : AIAttackState
{
    [Tooltip("The higher the value, the more the AI move back angle is affected by the distance between the player and the AI")]
    [SerializeField] private float omega = 0.5f;
    
    public override void MoveTo(NavMeshAgent agent, Vector3 destination)
    {
        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    public override AIState RunCurrentState(AIStateManager stateManager)
    {
        MoveBack(stateManager.player, stateManager.moveBackRange, stateManager.obstacleMask, stateManager.agent);
        
        if (Vector3.Distance(stateManager.player.position, transform.position) > stateManager.moveBackRange || 
            Physics.Raycast(transform.position, stateManager.player.position - transform.position, Vector3.Distance(transform.position, stateManager.player.position), stateManager.obstacleMask))
        {
            stateManager.agent.Stop();
            return previousState;
        }

        if (!Physics.Raycast(transform.position, stateManager.player.position - transform.position, Vector3.Distance(transform.position, stateManager.player.position), stateManager.obstacleMask))
        {
            transform.LookAt(stateManager.player);
            CheckAttack(stateManager.weaponData, stateManager.entity);
        }

        return null;
    }

    void MoveBack(Transform player, float moveBackRange, LayerMask obstacleMask, NavMeshAgent agent)
    {
        Vector3 initDirection = new Vector3((transform.position - player.position).x, 0.5f,(transform.position - player.position).z).normalized;
        Vector3 rotatedVector;
        float distance =  1 - Vector3.Distance(player.position, transform.position)/moveBackRange + omega;
        float angle = 2 * Mathf.Atan(Mathf.Sin(distance*Mathf.PI /2) / Mathf.Cos(distance*Mathf.PI /2)) * Mathf.Rad2Deg;
        for (int i = 0; i < 5; i++)
        {
            rotatedVector = (Quaternion.AngleAxis(angle/5 * i, Vector3.up) * initDirection).normalized;
            if (!Physics.Raycast(transform.position, rotatedVector, moveBackRange - Vector3.Distance(transform.position, player.position), obstacleMask))
            {
                MoveTo(agent,  transform.position + rotatedVector * moveBackRange);
                return;
            }
        }
        
        for (int i = 0; i < 5; i++)
        {
            rotatedVector = Quaternion.AngleAxis(angle/5 * -i, Vector3.up) * initDirection;
            if (!Physics.Raycast(transform.position, rotatedVector, moveBackRange - Vector3.Distance(transform.position, player.position), obstacleMask))
            {
                MoveTo(agent,  transform.position + rotatedVector * moveBackRange);
                return;
            }
        }

    }
}
