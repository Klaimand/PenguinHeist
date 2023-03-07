using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableEvent", menuName = "Scriptables/ScriptableEvent", order = 0)]
public class ScriptableEvent : ScriptableObject
{
    public string EventName => name;
    List<EventListener> eventListeners = new List<EventListener>();

    public void Invoke()
    {
        //Create copy in case some Listeners get removed
        List<EventListener> listeners = new List<EventListener>();
        foreach (EventListener l in eventListeners)
        {
            listeners.Add(l);
        }

        foreach (EventListener l in listeners)
        {
            l.OnEventCalled();
        }
    }

    public void RegisterListener(EventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(EventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}