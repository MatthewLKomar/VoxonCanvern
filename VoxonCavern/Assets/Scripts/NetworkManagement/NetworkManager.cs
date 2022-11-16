using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;


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

    

    public void Send(string text)
    {
        if (isClient)
        {
            client.Send(text);
        }
        else server.Send(text);
    }
}


