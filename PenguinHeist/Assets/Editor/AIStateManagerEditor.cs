using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(AIStateManager))]
public class AIStateManagerEditor : Editor
{
    private AIStateManager aiStateManager;
    private List<Transform> players;

    FloatField moveBackRange;

    public override void OnInspectorGUI()
    {
        CreateStates();
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        if (aiStateManager.agent == default || aiStateManager.weaponData == default)
        {
            return;
        }
        DrawPath();
        DrawDistance();
        DrawChaseDistance();
        DrawAttackRange();
        DrawMoveBackRange();
    }

    private void OnEnable()
    {
        aiStateManager = (AIStateManager)target;
        FindPlayers(1);
    }

    void DrawPath()
    {
        Handles.color = Color.blue;
        for (int i = 0; i < aiStateManager.agent.path.corners.Length - 1; i++)
        {
            Handles.DrawLine(aiStateManager.agent.path.corners[i], aiStateManager.agent.path.corners[i + 1]);
        }
    }
    
    void DrawDistance()
    {
        Handles.color = Color.red;
        if (players != default)
        {
            foreach (var player in players)
            {
                Handles.DrawLine(aiStateManager.transform.position, player.position);
                Handles.Label(aiStateManager.transform.position + Vector3.up,
                    "Distance : "+Vector3.Distance(aiStateManager.transform.position, player.position).ToString());
            }
        }
    }
    
    void DrawMoveBackRange()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(aiStateManager.transform.position, Vector3.up, aiStateManager.moveBackRange);
    }

    void DrawAttackRange()
    {
        Handles.color = new Color(1, 0.5f, 0);
        Handles.DrawWireDisc(aiStateManager.transform.position, Vector3.up, aiStateManager.attackRange);
    }
    
    void DrawChaseDistance()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireArc(aiStateManager.transform.position, Vector3.up, Vector3.forward, 360, aiStateManager.weaponData.range);
    }
    

    void FindPlayers(int layerNb)
    {
        players = new List<Transform>();
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
        foreach(GameObject go in gos)
        {
            if(go.layer==layerNb)
            {
                players.Add(go.transform);
            }
        } 
    }

    void CreateStates()
    {
        GUILayout.Label("Create States", style: EditorStyles.boldLabel);
        if (GUILayout.Button("Create Police"))
        {
            AIState[] aiStates = aiStateManager.gameObject.GetComponents<AIState>();

            foreach (var state in aiStates)
            {
                DestroyImmediate(state);
            }

            Awareness awareness = aiStateManager.GetComponent<Awareness>();
            
            DestroyImmediate(awareness);
            
            AIChaseState chaseState = CreateState<AIChaseState>();
            AIAttackState aiAttackState = CreateState<AIAttackState>();
            AIChaseAndAttackState chaseAndAttackState = CreateState<AIChaseAndAttackState>();
            
            FillStateVariables(chaseState, null, chaseAndAttackState, null);
            FillStateVariables(chaseAndAttackState, null, aiAttackState, chaseState);
            FillStateVariables(aiAttackState, null, null, chaseAndAttackState);
            
            aiStateManager.currentState = chaseState;
        }
        if (GUILayout.Button("Create Mafia"))
        {
            AIState[] aiStates = aiStateManager.gameObject.GetComponents<AIState>();

            foreach (var state in aiStates)
            {
                DestroyImmediate(state);
            }
            
            AIMoveState moveState = CreateState<AIMoveState>();
            Awareness awareness = CreateState<Awareness>();
            AIChaseState chaseState = CreateState<AIChaseState>();
            MafiaAgentAttackState aiAttackState = CreateState<MafiaAgentAttackState>();
            AIChaseAndAttackState chaseAndAttackState = CreateState<AIChaseAndAttackState>();
            AIMoveBackState moveBackState = CreateState<AIMoveBackState>();
            
            FillStateVariables(moveState, awareness, chaseState, null);
            FillStateVariables(chaseState, awareness, chaseAndAttackState, moveState);
            FillStateVariables(chaseAndAttackState, awareness, moveBackState, chaseState);
            FillStateVariables(aiAttackState, awareness, moveBackState, chaseAndAttackState);
            FillStateVariables(moveBackState, awareness, moveState, chaseAndAttackState);
            
            aiStateManager.currentState = moveState;
        }
    }
    
    T CreateState<T>() where T : MonoBehaviour
    {
        if (aiStateManager.gameObject.GetComponent<T>() == default)
        {
            return aiStateManager.gameObject.AddComponent<T>();
        }

        return null;
    }
    
    void FillStateVariables<T>(T state, Awareness awareness, AIState nextState, AIState previousState) where T : AIState
    { 
        if (typeof(T).IsAssignableFrom(typeof(AIMoveState)))
        {
            AIMoveState state2 = (AIMoveState)Convert.ChangeType(state, typeof(AIMoveState));
            state2.awareness = awareness;
        }

        if (nextState != default)
        {
            state.nextState = nextState;
        }

        if (previousState != default)
        {
            state.previousState = previousState;
        }
    }
}
