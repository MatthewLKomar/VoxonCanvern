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
    public Trigger EventTrigger;

    public bool button = false;

    private void Start()
    {
        objectManager = ObjectManager.current;
        network = NetworkManager.current;
    }
    public void Transmit()
    {
        string buffer = objectManager.BuildBufferGenericEvent
            (Command.GenericEvent,
            gameObject,
            EventTrigger.EventName,
            EventTrigger.CorrespondingEventID);
        EventTrigger.unityEvent.Invoke();

        network.Send(buffer);
    }
    // Update is called once per frame
    void Update()
    {
        if (button)
        {
            Transmit();
            button = false;
        }
    }
}
