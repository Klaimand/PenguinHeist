using UnityEngine;
using UnityEngine.AI;

public enum  AIStateType
{
    TPose,
    Idle,
    Walk,
    Death,
    Interact,
    Shoot,
    GunHold,
    Reload,
    HoldShield,
    Hit
}

public class AIStateManager : MonoBehaviour
{
    [Header("State")]
    [Tooltip("Current state of the AI and first state to run")]
    [SerializeField] public AIState currentState;
    [SerializeField] public AIStateType aIStateType;
    [Header("NavMesh")]
    public NavMeshAgent agent;
    [HideInInspector] public WeaponSO weaponData;
    [Header("Data")]
    [Tooltip("Distance to chase and attack the player")]
    public float chaseAndAttackRange;
    [Tooltip("Distance to attack the player")]
    public float attackRange;
    [Tooltip("Distance to move back when the player is too close( Only for mafia agent)")]
    public float moveBackRange;
    public Transform player;
    [Tooltip("Obstacles of the AI vision")]
    public LayerMask obstacleMask;
    public AIEntity entity;

    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        AIState nextState = currentState?.RunCurrentState(this);

        if (nextState != default)
        {
            SwitchToTheNextState(nextState);
        }
    }

    private void SwitchToTheNextState(AIState nextState)
    {
        currentState = nextState;
        aIStateType = currentState.aIStateType;
    }
}
