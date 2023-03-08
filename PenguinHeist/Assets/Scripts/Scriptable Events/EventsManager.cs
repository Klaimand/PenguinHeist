using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    [SerializeField] EventsList eventsList;

    public void TriggerEvent(string eName)
    {
        eventsList.GetEvent(eName)?.Invoke();
    }
}