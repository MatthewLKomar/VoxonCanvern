using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Text;
using UnityEngine;


//based on this:
// https://gist.github.com/louis-e/888d5031190408775ad130dde353e0fd
public class UDPSocket
{
    public Socket _socket;
    private const int bufSize = 120 * 1024;
    private State state = new State();
    private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
    private AsyncCallback recv = null;
        
    public string ipAdress = "127.0.0.1";
    public int port = 27000;


    public string output = "nothing";

    public class State
    {
        public byte[] buffer = new byte[bufSize];
    }

    public void Server()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        _socket.Bind(new IPEndPoint(IPAddress.Parse(ipAdress), port));
        Receive();
        NetworkManager.current.NetworkerPrint("Server is live!");
    }

    public void Client()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _socket.Connect(IPAddress.Parse(ipAdress), port);
        Receive();
        NetworkManager.current.NetworkerPrint("Client is live!");
    }

    // Transmit the data 
    public void Send(string text)
    {
        byte[] data = Encoding.ASCII.GetBytes(text);
        _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
        {
            State so = (State)ar.AsyncState;
            int bytes = _socket.EndSend(ar);
        }, state);
    }

    //Receive data 
    private void Receive()
    {
        _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
        {
            try
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                output = Encoding.ASCII.GetString(so.buffer, 0, bytes);
                // process the json buffer received
                ObjectManager.current.ProcessBuffer(output);
            }
            catch { }

        }, state);
    }
}

public class TCPLogic
{
    public string ipAdress = "127.0.0.1";
    public int port = 27000;
    public static int maxByteLength = 256;
    public string name;

}
public class TCPServer : TCPLogic
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
                NetworkManager.current.NetworkerPrint("Received: " + data);
                if (data != "Confirm")
                    ObjectManager.current.ProcessBuffer(data);
            }
        }
        catch (Exception e)
        {
            NetworkManager.current.NetworkerPrint("Exception: "+ e.ToString());
            client.Close();
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
            NetworkManager.current.NetworkerPrint("Sent: " + message);
            return true;
        }
        return false;
    }

}
public class TCPClient : TCPLogic
{
    TcpClient client;
    public TCPClient(string clientName)
    {
        name = clientName;
        new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            client = new TcpClient(ipAdress, port);
            StartListener();
        }).Start();
    }

    void StartListener()
    {
        try
        {
            while(true)
            {
                Thread t = new Thread(new ParameterizedThreadStart(ListenForData));
                t.Start();
            }
        }
        catch (Exception e)
        {
            NetworkManager.current.NetworkerPrint("Connect Exception: " + e);
        }
    }

    public void ListenForData(System.Object obj)
    {
        // Bytes Array to receive Server Response.
        try
        {
            NetworkStream stream = client.GetStream();
            Byte[] data = new Byte[maxByteLength];
            String response = String.Empty;

            //Read the Tcp Server Response Bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            response = Encoding.ASCII.GetString(data, 0, bytes);
            NetworkManager.current.NetworkerPrint(name + " Received: " + response);
            if (response != "connected")
                ObjectManager.current.ProcessBuffer(response);
        }
        catch ( Exception e)
        {
            NetworkManager.current.NetworkerPrint("Exception: " + e.ToString());
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
            NetworkManager.current.NetworkerPrint("Client Sent: " + message);

            stream.Close();
        }
        catch (Exception e)
        {
            NetworkManager.current.NetworkerPrint("Send Exception: " + e);
            Close();
        }
        
    }

    void Close()
    {
        client.Close();
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

    void Start()
    {
        server = new TCPServer("Voxon");
        client = new TCPClient("Cavern");
    }

    private void OnDestroy()
    {
        //server._socket.Close(); //Fixed closing bug (System.ObjectDisposedException)
        //Bugfix allows to relaunch server
    }

}


