using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackState : AIState
{
    [SerializeField] WeaponData weaponData;
    private float currentAttackCd;
    
    private void Start()
    {
        stateManager = GetComponent<AIStateManager>(); ;
    }
    
    public override void MoveTo(Vector3 destination)
    {
        
    }

    private void Update()
    {
        currentAttackCd -= Time.deltaTime;
        //transform.LookAt(player);
    }

    public override AIState RunCurrentState(NavMeshAgent agent)
    {
        if (agent != default)
        {
            stateManager.agent = agent;
        }
        
        if (Vector3.Distance(stateManager.player.position, transform.position) <= weaponData.range)
        {
            Attack();
        }
        else
        {
            agent.isStopped = false;
            agent.avoidancePriority = 1;
            return previousState;
        }
        return null;
    }

    private void Attack()
    {
        if (currentAttackCd <= 0)
        {
            currentAttackCd = weaponData.fireRate;
            StartCoroutine(Shoot());
        }
    }
    
    IEnumerator Shoot()
    {
        GameObject go = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), transform.position, Quaternion.identity);
        go.AddComponent<Rigidbody>();
        go.GetComponent<Rigidbody>().AddForce(50*transform.forward, ForceMode.Impulse);
        yield return new WaitForSeconds(weaponData.fireRate);
        Destroy(go);
    }
}
