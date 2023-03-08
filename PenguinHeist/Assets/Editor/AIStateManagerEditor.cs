using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[CustomEditor(typeof(AIStateManager))]
public class AIStateManagerEditor : Editor
{
    private AIStateManager aiStateManager;
    private List<Transform> players;
    
    [SerializeField] private VisualTreeAsset visualTree;

    private VisualElement root;
    
    FloatField moveBackRange;

    /*public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();
        root.Add(visualTree.Instantiate());

        Bind();

        return root;
    }*/
    
    private void Bind()
    {
        moveBackRange = root.Q<FloatField>("moveBackRange");
        moveBackRange.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            aiStateManager.moveBackRange = evt.newValue;
        });
        root.Q<PropertyField>("currentState").RegisterCallback<ChangeEvent<AIState>>(evt =>
        {
            aiStateManager.currentState = evt.newValue;
        });
        root.Q<PropertyField>("agent").RegisterCallback<ChangeEvent<NavMeshAgent>>(evt =>
        {
            aiStateManager.agent = evt.newValue;
        });
        root.Q<EnumField>("aiType").RegisterCallback<ChangeEvent<Enum>>(evt =>
        {
            aiStateManager.aiType = (AIType) evt.newValue;
            if (AIType.Police == (AIType) evt.newValue)
            {
                moveBackRange.visible = false;
            }
            else
            {
                moveBackRange.visible = true;
            }
        });
        root.Q<ObjectField>("weaponData").RegisterCallback<ChangeEvent<WeaponData>>(evt =>
        {
            aiStateManager.weaponData = evt.newValue;
        });
        root.Q<FloatField>("attackRange").RegisterCallback<ChangeEvent<float>>(evt =>
        {
            aiStateManager.attackRange = evt.newValue;
        });
    }
    
    /*public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CreateAwareness();
        CreateStates();
    }*/

    private void OnSceneGUI()
    {
        DrawPath();
        DrawDistance();
        Vector3 pos = Vector3.one;
        Quaternion rot = Quaternion.identity;
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

    void CreateAwareness()
    {
        GUILayout.Label("Awareness", style: EditorStyles.boldLabel);
        if (GUILayout.Button("Add Awareness"))
        {
            Awareness awareness = aiStateManager.gameObject.AddComponent<Awareness>();
            awareness.viewRadius = 10;
            awareness.viewAngle = 90;
        }
    }
    
    void CreateStates()
    {
        GUILayout.Label("States", style: EditorStyles.boldLabel);
        if (GUILayout.Button("Add Move State"))
        {
            CreateState<AIMoveState>();
        }
        if (GUILayout.Button("Add Chase State"))
        {
            CreateState<AIChaseState>();
        }
        if (GUILayout.Button("Add Attack State"))
        {
            CreateState<AIAttackState>();
        }
    }
    
    void CreateState<T>() where T : AIState
    {
        aiStateManager.gameObject.AddComponent<T>();
    }
}
