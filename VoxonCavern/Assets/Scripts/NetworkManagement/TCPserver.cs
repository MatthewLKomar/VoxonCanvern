using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Text;
using System.Collections;

public class TCPServer : TCPBase
{

    TcpListener server = null;
    TcpClient client = null;

    NetworkStream stream = null;

    //Creates the server on a new thread that is listening to any clients
    public TCPServer(string ServerName, string IP, int Port)
    {
        Name = ServerName;
        ipAdress = IP;
        port = Port;
        Thread t = new Thread(delegate ()
        {
            IPAddress localAddr = IPAddress.Parse(ipAdress);
            server = new TcpListener(localAddr, port);
            server.Start();
            StartListener();
        });
        t.Start();
    }

    //this will perpertually run to listen for an responses from the clients
    public void StartListener()
    {
        try
        {
            NetworkerPrint(Name + " is waiting for a connection");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkerPrint("Connected!");

                Thread t = new Thread(new ParameterizedThreadStart(ListenForData));
                t.Start(client);
            }
        }
        catch (SocketException e)
        {
            // something went wrong here...
            NetworkerPrint("SocketException:" + e);
            Close();
        }
    }

    public override void Close()
    {
        if (server != null)
        {
            Send("bye");
            server.Stop();
            server.Server.Close();
            if (stream != null)
                stream.Close();
        }
    }
    public IEnumerator ProcessBuffer(string data)
    {
        ObjectManager.current.ProcessBuffer(data);
        yield return null;
    }

    public void ListenForData(System.Object obj)
    {
        client = (TcpClient)obj;
        stream = client.GetStream();

        string response;
        Byte[] bytes = new Byte[maxByteLength];
        int i;
        try
        {
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                response = Encoding.ASCII.GetString(bytes, 0, i);
                NetworkerPrint(Name + " Received: " + response);
                Send("Server has recieved");
                if (response == "Bye")
                {
                    NetworkerPrint("Client disconnected, server shutting down");
                    Close();
                }
                //if (data != "Confirm")
                    //So unity isn't thread safe, so we have to use this tool to call things on the main thread.
                    //UnityMainThreadDispatcher.Instance().Enqueue(ProcessBuffer(data));
            }
        }
        catch (Exception e)
        {
            NetworkerPrint("Exception: " + e.ToString());
            Close();
        }
    }

    //this will send the message to the client 
    public bool Send(string message)
    {
        if (client != null)
        {
            stream = client.GetStream();
            //Send response to client here... 
            Byte[] reply = Encoding.ASCII.GetBytes(message);
            stream.Write(reply, 0, reply.Length);
            NetworkerPrint(Name + " Sent: " + message);
            return true;
        }
        return false;
    }

}