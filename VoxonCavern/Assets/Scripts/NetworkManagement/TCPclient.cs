using System;
using System.Net.Sockets;
using System.Threading;

using System.Text;
using System.Threading.Tasks;
public class TCPClient : TCPBase
{
    TcpClient client;
    NetworkStream stream;
    public TCPClient(string clientName, string IP, int Port)
    {
        Name = clientName;
        ipAdress = IP;
        port = Port;
        new Thread(() =>
        {
            try
            {
                Thread.CurrentThread.IsBackground = true;
                client = new TcpClient(ipAdress, port);
                var result = Listen();
                Send("Client: yo");
            }
            catch (Exception ex)
            {
                NetworkerPrint("error loading the client: " + ex.ToString());
            }

        }).Start();
    }

    public override void Close()
    {
        if (client != null)
        {
            client.Client.Close();
            if (stream != null)
                stream.Close();
        }
    }

    async Task Listen()
    {
        try
        {
            if (client.Connected)
            {
                stream = client.GetStream();

                while (client.Connected)
                {
                    NetworkerPrint("waiting for response");
                    byte[] buffer = new byte[maxByteLength];
                    int read = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        string response = Encoding.ASCII.GetString(buffer, 0, read);
                        NetworkerPrint(Name + " Received: " + response);
                        Send("Client says hello");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            NetworkerPrint("Client listening err:" + ex);
            client.Close();
        }
    }

    public void Send(string message)
    {
        try
        {
            stream = client.GetStream();
            // Translate the Message into ASCII.
            Byte[] data = Encoding.ASCII.GetBytes(message);

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);
            NetworkerPrint(Name + " sent: " + message);

        }
        catch (Exception e)
        {
            NetworkerPrint("Send Exception: " + e);
        }

    }

}
