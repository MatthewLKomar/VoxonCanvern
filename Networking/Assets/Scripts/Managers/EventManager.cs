using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public struct TriggerEvent
{
    public string name;
    public int ID;
    public UnityEvent unityEvent;
}

public class EventManager : MonoBehaviour
{
    [SerializeField, Tooltip("The events with the time to activate them.")]
    public List<TriggerEvent> Triggers;

    private int GetID(TriggerEvent trigger)
    {
        return trigger.ID;
    }

    private void Awake()
    {
        Triggers.Sort((FirstElem, SecondElem) => GetID(FirstElem) - GetID(SecondElem));
    }

    public bool triggerID(int ID)
    {
        //MKomar says... we have no guarantee that the IDs will be consecutive numbers
        foreach (var trigger in Triggers)
        {
            if (trigger.ID == ID)
            {
                trigger.unityEvent.Invoke();
                return true;
            }
        }
        return false;
    }

}
