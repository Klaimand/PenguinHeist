using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : MonoBehaviour
{
    [SerializeField] public AIState nextState;
    [SerializeField] public AIState previousState;
    public abstract void MoveTo(NavMeshAgent agent, Vector3 destination);
    public abstract AIState RunCurrentState(NavMeshAgent agent, Transform player, WeaponData weaponData, float attackRange, float moveBackRange, LayerMask obstacleMask);
}
