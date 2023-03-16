using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIAttackState : AIState
{
    [SerializeField] Transform canon;

    public Action OnEnemyShoot;

    protected RaycastHit hit;

    public AudioSource gunshotSfx;

    public override void MoveTo(NavMeshAgent agent, Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public override AIState RunCurrentState(AIStateManager stateManager)
    {
        if (!Physics.Raycast(transform.position, stateManager.player.position - transform.position, out hit, Vector3.Distance(transform.position, stateManager.player.position), stateManager.obstacleMask))
        {
            Vector3 lkvector = stateManager.player.position - transform.position;
            lkvector.y = 0f;
            transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( lkvector ), Time.deltaTime );
            
            CheckAttack(stateManager.weaponData, stateManager.entity, 1);
        }
        else
        {
            stateManager.agent.isStopped = false;
            stateManager.agent.avoidancePriority = 1;
            return previousState;
        }

        if (Vector3.Distance(transform.position, stateManager.player.position) > stateManager.attackRange)
        {
            stateManager.agent.isStopped = false;
            stateManager.agent.avoidancePriority = 1;
            return previousState;
        }
        IncreaseTimers(stateManager.entity);
        return null;
    }

    IEnumerator Attack(WeaponSO weaponData, AIEntity entity)
    {
        OnEnemyShoot?.Invoke(); //shoot

        for (int i = 0; i < weaponData.shotsPerClick; i++)
        {
            entity.curMagazineBullets--;
            Vector3 dir = (entity.aiStateManager.player.position + Vector3.up*0.5f) - canon.position;
            //  //dir.y = 0f;
            //
             float rdmAngle = Random.Range(-weaponData.spread / 2f, weaponData.spread / 2f);
            //
            dir = Quaternion.Euler(0f, rdmAngle, 0f) * dir;

            if (weaponData.gunshotSFX != null)
            {
                gunshotSfx.clip = weaponData.gunshotSFX; 
                gunshotSfx.Play();
            }
            
            Instantiate(weaponData.bulletPrefab, canon.position, Quaternion.LookRotation(dir));

            yield return new WaitForSeconds(weaponData.timeBetweenShots);
        }
    }

    protected void CheckAttack(WeaponSO weapon, AIEntity entity, int rr = 0)
    {
        if (entity.currentAttackCd < weapon.fireRate) return;

        if (entity.isReloading) return;

        if (LevelManager.instance.GetHealthFromTransform(entity.aiStateManager.player) == null) return;

        if (LevelManager.instance.GetHealthFromTransform(entity.aiStateManager.player).IsNotAlive)
        {
            if (entity.aiStateManager.player == LevelManager.instance.player1)
            {
                entity.aiStateManager.player = LevelManager.instance.player2;
            }
            else if (entity.aiStateManager.player == LevelManager.instance.player2)
            {
                entity.aiStateManager.player = LevelManager.instance.player1;
            }
            return;
        }
        
        if (entity.curMagazineBullets <= 0)
        {
            entity.isReloading = true;
            StartCoroutine(Reload(weapon, entity));
            return;
        }

        entity.currentAttackCd = 0f;
        StartCoroutine(Attack(weapon, entity));
    }

    void IncreaseTimers(AIEntity entity)
    {
        entity.currentAttackCd += Time.deltaTime;
    }

    IEnumerator Reload(WeaponSO weapon, AIEntity entity)
    {
        yield return new WaitForSeconds(weapon.reloadTime);
        entity.curMagazineBullets = weapon.bulletsPerMagazine;
        entity.isReloading = false;
    }
}
