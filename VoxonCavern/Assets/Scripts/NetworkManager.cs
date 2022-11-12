using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace NetworkManager
{
    //based on this:
    // https://gist.github.com/louis-e/888d5031190408775ad130dde353e0fd
    public class UDPSocket
    {
        public Socket _socket;
        private const int bufSize = 8 * 1024;
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
        }

        public void Client()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Connect(IPAddress.Parse(ipAdress), port);
            Receive();
        }

        // Transmit the data 
        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
                Console.WriteLine("SEND: {0}, {1}", bytes, text);
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
                    output = "RECV:" + Encoding.ASCII.GetString(so.buffer, 0, bytes);
                }
                catch { }

            }, state);
        }
    }

    public class NetworkManager : MonoBehaviour
    {
        // Start is called before the first frame update
        UDPSocket s;
        void Start()
        {
            s = new UDPSocket();
            s.Server();

            UDPSocket c = new UDPSocket();
            c.Client();
            c.Send("TEST!");

            
            //Console.ReadKey();
            //s._socket.Close(); //Fixed closing bug (System.ObjectDisposedException)
                               //Bugfix allows to relaunch server
            
        }

        private void Update()
        {
            if (s != null)
                print(s.output);
        }


    }



}
