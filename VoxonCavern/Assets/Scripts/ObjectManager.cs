using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager current;

    [HideInInspector, Tooltip("these are our tracked objects in the gameworld that will replicate across clients")]
    public Dictionary<string, GameObject> TrackedObjects = new Dictionary<string, GameObject>();

    public void ConfirmNetworkerIsRunning(string text)
    {
        print(text);
    }
    //validation that our Json systems are working. 
    void Start()
    {
        var payload = new Payload();
        payload.command = Command.Parent;
        payload.ObjectName = "TestObj";

        var asignee = new AssignParam();
        asignee.ParentObj = "ParentObj";
        payload.Params = ParamsToJson(payload.command,asignee);

        string json = JsonUtility.ToJson(payload);
        print(json);
        var test = JsonUtility.FromJson<Payload>(json);
        print("test");
        
        // JSON format
        /*{ 
            * "Items":[
            * { "command":1,
            *   "ObjectName":"Test",
            *   "Params":"empty"}
            * ]
        * }*/
    }

    public void ProcessBuffer(string json)
    {
        Payload buffer = JsonUtility.FromJson<Payload>(json);
        switch (buffer.command)
        {
            case Command.GenericEvent:
                CallGenericEvent();
                break;
            case Command.Spawn:
                Spawn(buffer.ObjectName);
                break;
            case Command.Parent:
                var ParentParam = JsonUtility.FromJson<AssignParam>(buffer.Params);
                Parent(buffer.ObjectName, ParentParam.ParentObj);
                break;
            case Command.Move:
                var MoveParam = JsonUtility.FromJson<Vector3Param>(buffer.Params);
                Move(buffer.ObjectName, MoveParam.vector3);
                break;
            case Command.Scale:
                var ScaleParam = JsonUtility.FromJson<Vector3Param>(buffer.Params);
                Scale(buffer.ObjectName, ScaleParam.vector3);
                break;
            case Command.Rotate:
                var RotationParam = JsonUtility.FromJson<QuaternionParam>(buffer.Params);
                Rotate(buffer.ObjectName, RotationParam.quaternion);
                break;
            default:
                break;
        }
    }

    void Spawn(string ObjName)
    {
        TrackedObjects.Add(ObjName, new GameObject(ObjName));
    }

    void TrackExistingItem(GameObject obj)
    {
        TrackedObjects.Add(obj.name,obj);
    }

    void Parent(string ObjName, string ParentName)
    {

        TrackedObjects.TryGetValue(ParentName, out GameObject parent);
        TrackedObjects.TryGetValue(ObjName, out GameObject child);
        child.transform.parent = parent.transform;
    }

    void Scale(string ObjName, Vector3 scale)
    {
        TrackedObjects.TryGetValue(ObjName, out GameObject obj);
        obj.transform.localScale = scale;
    }
    void Move(string ObjName, Vector3 position)
    {
        TrackedObjects.TryGetValue(ObjName, out GameObject obj);
        obj.transform.position = position;
    }

    void Rotate(string ObjName, Quaternion quaternion)
    {
        TrackedObjects.TryGetValue(ObjName, out GameObject obj);
        obj.transform.rotation = quaternion;
    }

    void CallGenericEvent()
    {
        //Event Manager work here
        print("TODO: Implement Call Generic Event");
    }

    string ParamsToJson<T>(Command command, T data)
    {
        string returnJson = ""; 

        switch (command)
        {
            case Command.GenericEvent:
                //I'm doing some really jank casting here with generic types, is this bad? 
                returnJson =  JsonUtility.ToJson(
                    (EventNameParam)Convert.ChangeType(data, typeof(EventNameParam))
                );
                break;
            case Command.Spawn:
                //no params needed to spawn
                returnJson = "";
                break;
            case Command.Parent:
                returnJson = JsonUtility.ToJson(
                    (AssignParam)Convert.ChangeType(data, typeof(AssignParam))
                );
                break;
            case Command.Move:
            case Command.Scale:
                // these two cases are combined
                returnJson = JsonUtility.ToJson(
                    (Vector3Param)Convert.ChangeType(data, typeof(Vector3Param))
                );
                break;
            case Command.Rotate:
                returnJson = JsonUtility.ToJson(
                    (QuaternionParam)Convert.ChangeType(data, typeof(QuaternionParam))
                );
                break;
            default:
                //WE SHOULD NEVER GET HERE
                returnJson = "";
                break;
        }
        return returnJson;
    }

    

}
