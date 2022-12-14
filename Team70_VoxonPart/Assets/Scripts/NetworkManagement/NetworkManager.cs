using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviour
{
    public static NetworkManager current;
    [Tooltip("the ip address the server will run on/client will connect to")]
    public string IPaddress = "127.0.0.1";
    [Tooltip("the port the server will run on/client will connect to")]
    public int Port = 27000;
    [Tooltip("Is this unity project a client or a server?")]
    public bool isClient = false;


    TCPServer server;
    TCPClient client;

    public Text DisplayOut;

    public void Awake()
    {
        if (current == null)
            current = this;
        else Destroy(this);

        if (isClient)
            client = new TCPClient("Cavern", IPaddress, Port);
        else
            server = new TCPServer("Voxon", IPaddress, Port);
    }

    private IEnumerator PrintDisplay(string text)
    {
        //DisplayOut.text = text;
        print(text);
        yield return null;
    }
    public void PrintAndDisplay(string text)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(PrintDisplay(text));
    }

    public void OnDestroy()
    {
        if (client != null)
        {
            client.Close();
        } else if (server != null)
        {
            server.Close();
        }

    }

    public void Send(string text)
    {
        if (isClient)
        {
            client.Send(text);
        }
        else server.Send(text);
    }
}


