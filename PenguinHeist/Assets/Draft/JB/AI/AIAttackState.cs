using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackState : AIState
{
    protected float currentAttackCd;
    protected RaycastHit hit;

    public override void MoveTo(NavMeshAgent agent, Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private void Update()
    {
        currentAttackCd -= Time.deltaTime;
    }

    public override AIState RunCurrentState(NavMeshAgent agent, Transform player, WeaponData weaponData, float attackRange, float moveBackRange, LayerMask obstacleMask)
    {
        if (!Physics.Raycast(transform.position, player.position - transform.position, out hit, Vector3.Distance(transform.position, player.position), obstacleMask))
        {
            transform.parent.LookAt(player);
            Attack(weaponData);
        }
        else
        {
            agent.isStopped = false;
            agent.avoidancePriority = 1;
            return previousState;
        }

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            agent.isStopped = false;
            agent.avoidancePriority = 1;
            return previousState;
        }
        return null;
    }

    protected void Attack(WeaponData weaponData)
    {
        if (currentAttackCd <= 0)
        {
            currentAttackCd = weaponData.fireRate;
            StartCoroutine(Shoot(weaponData));
        }
    }
    
    IEnumerator Shoot(WeaponData weaponData)
    {
        GameObject go = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), transform.position, Quaternion.identity);
        go.AddComponent<Rigidbody>();
        go.GetComponent<Rigidbody>().AddForce(50*transform.forward, ForceMode.Impulse);
        yield return new WaitForSeconds(weaponData.fireRate);
        Destroy(go);
    }
}
