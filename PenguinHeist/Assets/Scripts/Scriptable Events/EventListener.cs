using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField] ScriptableEvent Event;
    [SerializeField] UnityEvent onEventCalled;

    void OnEnable()
    {
        Event.RegisterListener(this);
    }

    void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventCalled()
    {
        onEventCalled.Invoke();
    }
}