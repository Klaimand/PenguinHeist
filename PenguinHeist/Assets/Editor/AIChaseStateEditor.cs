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
    }

    private void OnSceneGUI()
    {
        DrawChaseDistance();
    }

    void DrawChaseDistance()
    {
        Handles.color = Color.black;
        Handles.DrawWireArc(aiChaseState.transform.position, Vector3.up, Vector3.forward, 360, aiChaseState.weaponData.range);
    }
}
