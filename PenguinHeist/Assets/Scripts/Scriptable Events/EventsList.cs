using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableEventsList", menuName = "Scriptables/ScriptableEventsList", order = 0)]
public class EventsList : ScriptableObject
{
    [SerializeField]
    private ScriptableEvent[] events;

    public ScriptableEvent GetEvent(string eventName)
    {
        foreach (var e in events)
        {
            if (e.EventName == eventName) return e;
        }

        Debug.LogWarning(eventName + " is not a referenced Event.");
        return null;
    }
}