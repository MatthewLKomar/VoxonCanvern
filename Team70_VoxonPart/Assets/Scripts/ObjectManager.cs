using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager current;
    public NetworkManager networker; 
    public bool start = false;
    [Tooltip("Objects here will be replicated")]
    public List<GameObject> ObjectsToReplicate = new List<GameObject>();

    [HideInInspector, Tooltip("these are our tracked objects in the gameworld that will replicate across clients")]
    public Dictionary<string, GameObject> TrackedObjects = new Dictionary<string, GameObject>();

    public void Awake()
    {
        //makes this game object a publicly visible object 
        if (current == null)
        {
            current = this;
        }
        else Destroy(gameObject);
    }
    public void ConfirmNetworkerIsRunning(string text)
    {
        print(text);
    }

    void SendInitialData(GameObject obj)
    {
        //spawn
        var buffer = BuildBufferSpawn(Command.Spawn, obj);
        networker.Send(buffer);
        //move
        networker.Send(BuildBufferVector3(Command.Move, obj, obj.transform.position));
        //rotate
        networker.Send(BuildBufferRotation(Command.Rotate, obj, obj.transform.rotation));
        //scale
        networker.Send(BuildBufferVector3(Command.Scale, obj, obj.transform.localScale));
    }

    void TraverseHeirarchy(Transform root)
    {
        TrackedObjects.Add(root.name, root.gameObject);
        
        SendInitialData(root.gameObject);
        
        for (int i = 0; i < root.transform.childCount; i++)
        {
            TraverseHeirarchy(root.transform.GetChild(i));
        }
    }

    IEnumerator InitializeTrackedObjs()
    {
        foreach (var obj in ObjectsToReplicate)
        {
            TraverseHeirarchy(obj.transform);
        }
        yield return null;
    }

    IEnumerator TrackAllObjects(float SecondsToWait, GameObject toDisable)
    {

        //yield return new WaitForSeconds(SecondsToWait);
        //not yet implemented
        yield return null;
    }
    
    private void Update()
    {
        if (start)
        {
            StartCoroutine(InitializeTrackedObjs());
            start = false;
        }
    }

    public void ProcessBuffer(string json)
    {
        Payload buffer = JsonUtility.FromJson<Payload>(json);
        print(buffer.command);
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

        //bool objExist = TrackedObjects.TryGetValue(ObjName, out var objToSpawn);
            
        var objToSpawn = new GameObject();
        objToSpawn.name = ObjName;

        TrackedObjects.Add(objToSpawn.name, objToSpawn);
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

    Payload CreateEmptyPayload(Command command, GameObject obj)
    {
        var payload = new Payload();
        payload.command = command;
        payload.ObjectName = obj.name;
        return payload;
    }

    //[Tooltip("Create the JSON buffer to send to the network")]
    string BuildBufferForGenericEvent(Command command, GameObject obj, string Event)
    {
        var payload = CreateEmptyPayload(command, obj);
        payload.Params = GenericEventToJson(Event);
        return JsonUtility.ToJson(payload);
    }

    string BuildBufferVector3(Command command, GameObject obj, Vector3 Event)
    {
        var payload = CreateEmptyPayload(command, obj);
        payload.Params = Vector3ToJson(Event);
        return JsonUtility.ToJson(payload);
    }

    string BuildBufferSpawn(Command command, GameObject obj)
    {
        var payload = CreateEmptyPayload(command, obj);
        payload.Params = "";
        return JsonUtility.ToJson(payload);
    }

    string BuildBufferRotation(Command command, GameObject obj, Quaternion Event)
    {
        var payload = CreateEmptyPayload(command, obj);
        payload.Params = RotateToJson(Event);
        return JsonUtility.ToJson(payload);
    }

    string BuildBufferParent(Command command, GameObject obj, string Event)
    {
        var payload = CreateEmptyPayload(command, obj);
        payload.Params = ParentToJson(Event);
        return JsonUtility.ToJson(payload);
    }



    string GenericEventToJson(string Event)
    {
        var GenericEvent = new EventNameParam();
        GenericEvent.EventName = Event;
        return JsonUtility.ToJson(GenericEvent);
    }

    string Vector3ToJson(Vector3 v3)
    {
        var vector3Param = new Vector3Param();
        vector3Param.vector3 = v3;
        return JsonUtility.ToJson(vector3Param);
    }

    string RotateToJson(Quaternion rotator)
    {
        var quaternionParam = new QuaternionParam();
        quaternionParam.quaternion = rotator;
        return JsonUtility.ToJson(quaternionParam);
    }

    string ParentToJson(string assigned)
    {
        var parenting = new AssignParam();
        parenting.ParentObj = assigned; 
        return JsonUtility.ToJson(parenting);
    }

}
