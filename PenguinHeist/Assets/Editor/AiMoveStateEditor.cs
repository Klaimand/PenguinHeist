using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(AIMoveState))]
public class AiMoveStateEditor : Editor
{
    AIMoveState aiMoveState;
    private NavMeshAgent agent;
    
    private void OnEnable()
    {
        aiMoveState = (AIMoveState)target;
        //agent = aiMoveState.transform.parent.GetComponent<NavMeshAgent>();
    }

    private void OnSceneGUI()
    {
        if (aiMoveState.wayPoints == default)
        {
            return;
        }
        for (int i = 0; i < aiMoveState.wayPoints.Length; i++)
        {
            Vector3 pos = Handles.PositionHandle(aiMoveState.wayPoints[i], Quaternion.identity);
            aiMoveState.wayPoints[i] = new Vector3(pos.x, 0.5f, pos.z);
            Handles.Label(aiMoveState.wayPoints[i], "WayPoint " + i);
        }

        if (Application.isPlaying)
        {
            NavMeshPath path;
            GameObject go;

            //Calculate Path in editor
            
            for (int i = 0; i < aiMoveState.wayPoints.Length - 1; i++)
            {
                go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                agent = go.AddComponent<NavMeshAgent>();
                go.transform.position = aiMoveState.wayPoints[i];
                path = new NavMeshPath();
                agent.CalculatePath(aiMoveState.wayPoints[i+1], path);
                Handles.DrawPolyLine(path.corners);
                Destroy(go);
            }
            go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            agent = go.AddComponent<NavMeshAgent>();
            go.transform.position = aiMoveState.wayPoints[^1];
            path = new NavMeshPath();
            agent.CalculatePath(aiMoveState.wayPoints[0], path);
            Handles.DrawPolyLine(path.corners);
            Destroy(go);
        }
    }
}
