using UnityEngine;
using UnityEngine.AI;

public class MafiaAgentAttackState : AIAttackState
{
    public override AIState RunCurrentState(AIStateManager stateManager)
    {
        if (Vector3.Distance(stateManager.player.position , transform.position) < stateManager.moveBackRange)
        {
            return nextState;
        }
        return base.RunCurrentState(stateManager);
    }
}
