using UnityEngine;
using UnityEngine.AI;

public class AIMoveBackState : AIAttackState
{
    [SerializeField] private float omega;
    
    public override void MoveTo(NavMeshAgent agent, Vector3 destination)
    {
        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    public override AIState RunCurrentState(NavMeshAgent agent, Transform player, WeaponData weaponData, float attackRange,
        float moveBackRange, LayerMask obstacleMask)
    {
        MoveBack(player, moveBackRange, obstacleMask, agent);
        
        if (Vector3.Distance(player.position, transform.position) > moveBackRange || 
            Physics.Raycast(transform.position, player.position - transform.position, Vector3.Distance(transform.position, player.position), obstacleMask))
        {
            agent.Stop();
            return previousState;
        }

        if (!Physics.Raycast(transform.position, player.position - transform.position, Vector3.Distance(transform.position, player.position), obstacleMask))
        {
            transform.parent.LookAt(player);
            Attack(weaponData);
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
