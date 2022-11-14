using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCPBase : MonoBehaviour
{
    public string ipAdress = "127.0.0.1";
    public int port = 27000;
    public static int maxByteLength = 1024;
    public string Name;

    public void NetworkerPrint(string text)
    {
        print(text);
    }
}

public enum Command
{
    [Tooltip("A Generic Event will cause a Unity Event call to be fired when received")]
    GenericEvent,
    [Tooltip("Creates the object on the other end")]
    Spawn,
    [Tooltip("Parents an object to another one.")]
    Parent,
    [Tooltip("Will move a corresponding object name to an associated point.")]
    Move,
    [Tooltip("Will scale the object")]
    Scale,
    [Tooltip("Will rotate the object to a provided value")]
    Rotate
}

[System.Serializable]
public struct Vector3Param
{
    public Vector3 vector3; 
}

[System.Serializable]
public struct QuaternionParam
{
    public Quaternion quaternion;
}

[System.Serializable]
public struct AssignParam
{
    public string ParentObj;
}

[System.Serializable]
public struct EventNameParam
{
    public string EventName;
}

[System.Serializable]
public struct Payload
{
    [Tooltip("Transmission Type")]
    public Command command;
    [Tooltip("Who will the data be directed towards")]
    public string ObjectName; 
    [Tooltip("Serialized JSON data and its data type will be relative to the command")]
    public string Params;  


}

