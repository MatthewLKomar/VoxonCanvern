using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransmissionType
{
    [Tooltip("A Generic Event will cause a Unity Event call to be fired when received")]
    GenericEvent,
    [Tooltip("this will set something associate Vector3 values to the value defined in event.")]
    Vector3Event
}

public struct Payload
{
    [Tooltip("Transmission Type")]
    public TransmissionType dataType;
    [Tooltip("Who will the data be directed towards")]
    public string Context; 
    [Tooltip("data will be forammted dependent on the dataType")]
    public string data;  
}