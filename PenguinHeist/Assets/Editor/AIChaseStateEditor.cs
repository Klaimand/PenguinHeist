using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIChaseState))]
public class AIChaseStateEditor : Editor
{
    AIChaseState aiChaseState;
    private void OnEnable()
    {
        aiChaseState = (AIChaseState)target;
        aiChaseState.stateManager = aiChaseState.transform.GetComponent<AIStateManager>();
    }

    private void OnSceneGUI()
    {
        DrawChaseDistance();
    }

    void DrawChaseDistance()
    {
        Handles.color = Color.black;
        Handles.DrawWireArc(aiChaseState.transform.position, Vector3.up, Vector3.forward, 360, aiChaseState.stateManager.weaponData.range);
    }
}
