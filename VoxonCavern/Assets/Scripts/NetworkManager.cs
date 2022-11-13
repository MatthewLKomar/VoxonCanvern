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

public class TCPServer
{
    public string ipAdress = "127.0.0.1";
    public int port = 27000;

    TcpListener server = null;
    public TCPServer()
    {
        IPAddress localAddr = IPAddress.Parse(ipAdress);
        server = new TcpListener(localAddr, port);
        server.Start();
        StartListener();
    }

    public void StartListener()
    {
        try
        {
            while (true)
            {
                NetworkManager.current.NetworkerPrint("Waiting for a connection...");
                TcpClient client = server.AcceptTcpClient();
                NetworkManager.current.NetworkerPrint("Connected!");

                Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                t.Start(client);
            }
        }
        catch (SocketException e)
        {
            NetworkManager.current.NetworkerPrint("SocketException:" + e);
            server.Stop();
        }
    }

    public void HandleDeivce(System.Object obj)
    {
        TcpClient client = (TcpClient)obj;
        var stream = client.GetStream();
        string imei = String.Empty;

        string data = null;
        Byte[] bytes = new Byte[256];
        int i;
        try
        {
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                string hex = BitConverter.ToString(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, i);
                NetworkManager.current.NetworkerPrint("Received: " + data);

                string str = "Hey Device!";
                Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);
                stream.Write(reply, 0, reply.Length);
                NetworkManager.current.NetworkerPrint("Sent: " + str);
            }
        }
        catch (Exception e)
        {
            NetworkManager.current.NetworkerPrint("Exception: "+ e.ToString());
            client.Close();
        }
    }
}
public class TCPSClient
{
    public string ipAdress = "127.0.0.1";
    public int port = 27000;

    public void Test()
    {
        new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            Connect(ipAdress, "Hello I'm Device 1...");
        }).Start();

        new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            Connect(ipAdress, "Hello I'm Device 2...");
        }).Start();


        Console.ReadLine();
    }

    void Connect(String server, String message)
    {
        try
        {
            TcpClient client = new TcpClient(server, port);

            NetworkStream stream = client.GetStream();

            int count = 0;
            while (count++ < 3)
            {
                // Translate the Message into ASCII.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);

                // Bytes Array to receive Server Response.
                data = new Byte[256];
                String response = String.Empty;

                // Read the Tcp Server Response Bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", response);

                Thread.Sleep(2000);
            }

            stream.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e);
        }

        Console.Read();
    }
}

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager current;

    public bool isClient = true; 
    UDPSocket server;
    UDPSocket client;

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
        /*        server = new UDPSocket();
                server.Server();

                client = new UDPSocket();
                client.Client();*/
        //c.Send("TEST!");
        Thread t = new Thread(delegate ()
        {
            // replace the IP with your system IP Address...
            TCPServer myserver = new TCPServer();

        });
        t.Start();

        TCPSClient tCPSClient = new TCPSClient();
        tCPSClient.Test();
    }

    private void OnDestroy()
    {
        //server._socket.Close(); //Fixed closing bug (System.ObjectDisposedException)
        //Bugfix allows to relaunch server
    }

}


