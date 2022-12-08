using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * MKomar Says... for the future, instead of hardcoding functions here do these through 
 * delegate events and a function elsewhere can bind to this and listen to it. 
 * 
 * So if you have multiple listeners they each can choose what to implement!
 */


[RequireComponent(typeof(EventManager))]
public class ObjectManager : MonoBehaviour
{
    public static ObjectManager current;

    private NetworkManager networker = NetworkManager.current;
    private EventManager eventManager; 
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

        eventManager = GetComponent<EventManager>();
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
    

    /*
        Takes in a param: json
        triggers functions based on the commands inside the json
     */ 
    public void ProcessBuffer(string json)
    {
        string[] commands = json.Split('\n');
        foreach(var command in commands)
        {
            if (!command.StartsWith("{")) continue; 

            Payload buffer = JsonUtility.FromJson<Payload>(command);
            switch (buffer.command)
            {
                case Command.GenericEvent:
                    CallGenericEvent(buffer.Params);
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
                case Command.PuzzlesVisible:
                    var PuzzleVisibleParam = JsonUtility.FromJson<VisiblePuzzles>(buffer.Params);
                    VisualizeVisiblePuzzles(PuzzleVisibleParam);
                    break;
                default:
                    break;
            }
        }
    }

    void VisualizeVisiblePuzzles(VisiblePuzzles visiblePuzzles)
    {
        //TODO: Implementation
        //MLKomar says... 
        /*
         * This can be entirely hard coded where you can tag parts of the cube into an array
         * Then just trigger them to be lit
         */

        if (visiblePuzzles.numberOfVisiblePuzzles == 0) return;
        if (visiblePuzzles.numberOfVisiblePuzzles == 1)
        {
            // do something with visiblePuzzles.visiblePuzzles.puzzleID1
            return;
        }

        if (visiblePuzzles.numberOfVisiblePuzzles == 2)
        {
            // do something with visiblePuzzles.visiblePuzzles.puzzleID1
            // do something with visiblePuzzles.visiblePuzzles.puzzleID2
            return;
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

    /* Mkomar says... 
         The Call Generic Event recieves PayloadParams in JSON format. 
            It will convert it into a sturct and then it will call a corresponding
            event from the event manager 
         The event manager is expected to have the correct ID number inside it,
         otherwise this call will be ignored and logged into console. 
    */
    void CallGenericEvent(string PayloadParams)
    {
        EventNameParam ParentParam = JsonUtility.FromJson<EventNameParam>(PayloadParams);
        eventManager.triggerID(ParentParam.eventID);
    }

    Payload CreateEmptyPayload(Command command, string objName)
    {
        var payload = new Payload();
        payload.command = command;
        payload.ObjectName = objName;
        return payload;
    }

    string FormatPayload(Payload payload)
    {
        return JsonUtility.ToJson(payload) + "\n";
    }

    //[Tooltip("Create the JSON buffer to send to the network")]
    public string BuildBufferGenericEvent(Command command, GameObject obj, string Event, int ID)
    {
        var payload = CreateEmptyPayload(command, obj.name);
        payload.Params = GenericEventToJson(Event, ID);
        return FormatPayload(payload);
    }

    public string BuildBufferVector3(Command command, GameObject obj, Vector3 Event)
    {
        var payload = CreateEmptyPayload(command, obj.name);
        payload.Params = Vector3ToJson(Event);
        return FormatPayload(payload);
    }

    string VisiblePuzzlesToJson(int PuzzleVisible, int Puzzle1, int Puzzle2)
    {
        VisiblePuzzles visiblePuzzles = new VisiblePuzzles();
        visiblePuzzles.puzzleID1 = Puzzle1;
        visiblePuzzles.puzzleID2 = Puzzle2;
        visiblePuzzles.numberOfVisiblePuzzles = PuzzleVisible;

        return JsonUtility.ToJson(visiblePuzzles);
    }
    
    // networker.Send(BuildBufferPuzzleVisibile(2, 2, 1));
    public string BuildBufferPuzzleVisibile(int PuzzleVisible, int Puzzle1, int Puzzle2)
    {
        var payload = CreateEmptyPayload(Command.PuzzlesVisible, "Puzzle");
        payload.Params = VisiblePuzzlesToJson(PuzzleVisible, Puzzle1, Puzzle2);
        return FormatPayload(payload);
    }
    
    public string BuildBufferSpawn(Command command, GameObject obj)
    {
        var payload = CreateEmptyPayload(command, obj.name);
        payload.Params = "";
        return FormatPayload(payload);
    }

    public string BuildBufferRotation(Command command, GameObject obj, Quaternion Event)
    {
        var payload = CreateEmptyPayload(command, obj.name);
        payload.Params = RotateToJson(Event);
        return FormatPayload(payload);
    }

    public string BuildBufferParent(Command command, GameObject obj, string Event)
    {
        var payload = CreateEmptyPayload(command, obj.name);
        payload.Params = ParentToJson(Event);
        return FormatPayload(payload);
    }



    string GenericEventToJson(string Event, int ID)
    {
        var GenericEvent = new EventNameParam();
        GenericEvent.EventName = Event;
        GenericEvent.eventID = ID;
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
