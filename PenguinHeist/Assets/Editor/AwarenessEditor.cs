using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CustomEditor (typeof (Awareness),true)]
public class AwarenessEditor : Editor
{
    
#if UNITY_EDITOR

    void OnSceneGUI() {
        Display();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    public virtual void Display()
    {
        var awareness = (Awareness)target;
        Handles.color = Color.white;
        Handles.DrawWireArc (awareness.transform.position, Vector3.up, Vector3.forward, 360, awareness.viewRadius);
        Vector3 viewAngleA = awareness.DirFromAngle (-awareness.viewAngle / 2, false);
        Vector3 viewAngleB = awareness.DirFromAngle (awareness.viewAngle / 2, false);

        Handles.DrawLine (awareness.transform.position, awareness.transform.position + viewAngleA * awareness.viewRadius);
        Handles.DrawLine (awareness.transform.position, awareness.transform.position + viewAngleB * awareness.viewRadius);

        if (awareness.visibleTargets == default)
        {
            return;
        }
        
        Handles.color = Color.red;
        if (awareness.visibleTargets.Count > 0)
        {
            for (int i = 0; i < awareness.visibleTargets.Count; i++)
            {
                Handles.DrawLine (awareness.transform.position, awareness.visibleTargets[i].position);
            }
        }
    }

#endif
}
