using System;
using UnityEngine;
using UnityEngine.AI;

public enum  AIStateType
{
    TPose,
    Idle,
    Walk,
    Death,
    Interact,
    GunHold,
    HoldShield
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
    [SerializeField] public AIStateType aIStateType;
    [SerializeField] public AIType aiType = AIType.Mafia;
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
    [SerializeField] AITakeBagState takeBagState;

    private void Start()
    {
        ChooseClosestPlayer();
        agent.speed = currentState.speed;
    }

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
        agent.speed = currentState.speed;
        if (currentState.aIStateType != AIStateType.TPose)
        {
            aIStateType = currentState.aIStateType;
        }
    }

    public void SwitchToTakeBag(Transform bag)
    {
        aIStateType = AIStateType.Interact;
        currentState = takeBagState;
        takeBagState.Init(agent, bag);
    }
    
    void ChooseClosestPlayer()
    {
        agent.SetDestination(LevelManager.instance.player1.position);
        float distanceToPlayer1 = agent.remainingDistance;
        agent.SetDestination(LevelManager.instance.player2.position);
        float distanceToPlayer2 = agent.remainingDistance;
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
