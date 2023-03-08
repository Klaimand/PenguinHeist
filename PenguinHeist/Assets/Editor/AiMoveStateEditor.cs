using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIMoveState))]
public class AiMoveStateEditor : Editor
{
    AIMoveState aiMoveState;
    
    private void OnEnable()
    {
        aiMoveState = (AIMoveState)target;
    }

    private void OnSceneGUI()
    {
        for (int i = 0; i < aiMoveState.wayPoints.Count; i++)
        {
            Vector3 pos = Handles.PositionHandle(aiMoveState.wayPoints[i], Quaternion.identity);
            aiMoveState.wayPoints[i] = new Vector3(pos.x, 0.5f, pos.z);
        }
    }
}
