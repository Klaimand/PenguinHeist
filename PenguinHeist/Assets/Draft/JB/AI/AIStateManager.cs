using System;
using UnityEngine;
using UnityEngine.AI;

public enum AIStateType
{
    TPose,
    Idle = 1,
    Walk = 2,
    Death = 3,
    Interact = 5,
    //GunHold,
    //HoldShield
    WalkBack = 17
}

public enum AIType
{
    Police,
    Mafia
}

public class AIStateManager : MonoBehaviour
{
    [Header("State")]
    [Tooltip("Current state of the AI and first state to run")]
    [SerializeField] public AIState currentState;
    [SerializeField] public AIStateType aIStateType; //a relier aux anims
    [SerializeField] public AIType aiType = AIType.Mafia;
    [HideInInspector] public AIChaseState chaseState;
    [HideInInspector] public AIMoveState moveState;
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
    [HideInInspector] public Transform player;
    [Tooltip("Obstacles of the AI vision")]
    public LayerMask obstacleMask;
    public AIEntity entity;
    [HideInInspector] public AITakeBagState takeBagState;

    private void Start()
    {
        ChooseClosestPlayer();
        agent.speed = currentState.speed;
        chaseState = GetComponent<AIChaseState>();
        takeBagState = GetComponent<AITakeBagState>();
        moveState = GetComponent<AIMoveState>();
    }

    void Update()
    {
        if (entity.IsDead) return;

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

    public void SwitchToTheNextState(AIState nextState)
    {
        currentState = nextState;
        agent.speed = currentState.speed;
        if (currentState.aIStateType != AIStateType.TPose)
        {
            aIStateType = currentState.aIStateType;
        }
    }

    public void SwitchToTakeBag(Transform bag)
    {
        takeBagState.Init(agent, bag);
        SwitchToTheNextState(takeBagState);
    }

    public void ChooseClosestPlayer()
    {
        //agent.SetDestination(LevelManager.instance.player1.position);
        float distanceToPlayer1 = Vector3.Distance(transform.position, LevelManager.instance.player1.position);
        //agent.SetDestination(LevelManager.instance.player2.position);
        float distanceToPlayer2 = Vector3.Distance(transform.position, LevelManager.instance.player2.position);
        if (distanceToPlayer1 < distanceToPlayer2)
        {
            player = LevelManager.instance.player1;
        }
        else
        {
            player = LevelManager.instance.player2;
        }
    }
}
