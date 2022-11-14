using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Text;
using UnityEngine;
using System.Threading.Tasks;

public class TCPBase
{
    public string ipAdress = "127.0.0.1";
    public int port = 27000;
    public static int maxByteLength = 256;
    public string name;

}
public class TCPServer : TCPBase
{

    TcpListener server = null;
    TcpClient client = null;

    //Creates the server on a new thread that is listening to any clients
    public TCPServer(string ServerName)
    {
        name = ServerName;
        Thread t = new Thread(delegate ()
        {
            IPAddress localAddr = IPAddress.Parse(ipAdress);
            server = new TcpListener(localAddr, port);
            server.Start();
            StartListener();
        });
        t.Start();
    }

    public void StartListener()
    {
        try
        {
            NetworkManager.current.NetworkerPrint(name + " is waiting for a connection");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkManager.current.NetworkerPrint("Connected!");

                Thread t = new Thread(new ParameterizedThreadStart(ListenForData));
                t.Start(client);
            }
        }
        catch (SocketException e)
        {
            // something went wrong here...
            NetworkManager.current.NetworkerPrint("SocketException:" + e);
            server.Stop();
        }
    }

    public void ListenForData(System.Object obj)
    {
        client = (TcpClient)obj;
        var stream = client.GetStream();

        string data;
        Byte[] bytes = new Byte[maxByteLength];
        int i;
        try
        {
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = Encoding.ASCII.GetString(bytes, 0, i);
                NetworkManager.current.NetworkerPrint(name + " Received: " + data);
                if (data != "Confirm")
                    ObjectManager.current.ProcessBuffer(data);
            }
        }
        catch (Exception e)
        {
            NetworkManager.current.NetworkerPrint("Exception: "+ e.ToString());
        }
    }

    public bool Send(string message)
    {
        if (client != null)
        {
            var stream = client.GetStream();
            //Send response to client here... 
            Byte[] reply = Encoding.ASCII.GetBytes(message);
            stream.Write(reply, 0, reply.Length);
            NetworkManager.current.NetworkerPrint(name + " Sent: " + message);
            return true;
        }
        return false;
    }

}
public class TCPClient : TCPBase
{
    TcpClient client;
    public TCPClient(string clientName)
    {
        name = clientName;
        new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            client = new TcpClient(ipAdress, port);
            var result = Listen();
            Send("Confirm");

        }).Start();
    }

    async Task Listen()
    {
        try
        {

            if (client.Connected)
            {
                NetworkStream stream = client.GetStream();

                while (client.Connected)
                {
                    NetworkManager.current.NetworkerPrint("waiting for response");
                    byte[] buffer = new byte[maxByteLength];
                    int read = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        string response = Encoding.ASCII.GetString(buffer, 0, read);
                        NetworkManager.current.NetworkerPrint(name + " Received: " + response);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            NetworkManager.current.NetworkerPrint("Client listening err:" + ex);
            client.Close();
        }
    }

    public void Send(string message)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            // Translate the Message into ASCII.
            Byte[] data = Encoding.ASCII.GetBytes(message);

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);
            NetworkManager.current.NetworkerPrint(name + " sent: " + message);

        }
        catch (Exception e)
        {
            NetworkManager.current.NetworkerPrint("Send Exception: " + e);
        }
        
    }

}

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager current;

    public bool isClient = false;
    TCPServer server;
    TCPClient client;

    public void Awake()
    {
        //makes this game object a publicly visible object 
        if (current == null)
        {
            current = this;
        }
        else Destroy(gameObject);

        server = new TCPServer("Voxon");
        client = new TCPClient("Cavern");
    }

    public void NetworkerPrint(string text)
    {
        print(text);
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


