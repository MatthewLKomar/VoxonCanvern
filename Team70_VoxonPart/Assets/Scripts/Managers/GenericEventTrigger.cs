using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




[System.Serializable]
public struct Trigger
{
    [Tooltip("this is only for designers to use and has no functional affect")]
    public string EventName;
    [Tooltip("Have this ID match the ID for a corresponding event on the receiver")]
    public int CorrespondingEventID;
    [Tooltip("Fire any events locally if need be during completion of this event")]
    public UnityEvent unityEvent;
}

public class GenericEventTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private NetworkManager network;
    private ObjectManager objectManager;
    
    [SerializeField, Tooltip("The events with the time to activate them.")]
    public List<Trigger> Triggers;

    private void Start()
    {
        objectManager = ObjectManager.current;
        network = NetworkManager.current;
    }
    public void Transmit(int index)
    {
        string buffer = objectManager.BuildBufferGenericEvent
            (Command.GenericEvent,
            gameObject,
            Triggers[index].EventName,
            Triggers[index].CorrespondingEventID);
        Triggers[index].unityEvent.Invoke();

        network.Send(buffer);
    }
   
}
