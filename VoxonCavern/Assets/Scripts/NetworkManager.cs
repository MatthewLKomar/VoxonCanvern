using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;

namespace NetworkManager
{
    public class UDPServer
    {
        public const int PORT = 5000;

        private Socket _socket; 
        private EndPoint _endPoint;

        private byte[] _buffer_recv;
        private ArraySegment<byte> _buffer_recv_segment;

        public void Initialize()
        {
            _buffer_recv = new byte[4096];
            _buffer_recv_segment = new(_buffer_recv);

            _endPoint = new IPEndPoint(IPAddress.Any, PORT);

            _socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);

            _socket.Bind(_endPoint);
        }

        public void StartMessageLoop()
        {
            //replace this with coroutine
            _ = Task.Run(async () =>
            {
                SocketReceiveMessageFromResult res;
                while (true)
                {
                    res = await _socket.ReceiveMessageFromAsync(_buffer_recv_segment, SocketFlags.None, _endPoint);
                    Console.WriteLine($"Recieved message: " +
                        $"{Encoding.UTF8.GetString(_buffer_recv,0,res.ReceivedBytes)}");
                    await SendTo(res.RemoteEndPoint, Encoding.UTF8.GetBytes("Hello Back!"));
                }
            }
            );
        }

        public async Task SendTo(EndPoint recipient, byte[] data)
        {
            var s = new ArraySegment<byte>(data);
            await _socket.SendToAsync(s, SocketFlags.None, recipient);
        }

    }
    public class UDPClient
    {
        public const int PORT = 5000;

        private Socket _socket;
        private EndPoint _endPoint;

        private byte[] _buffer_recv;
        private ArraySegment<byte> _buffer_recv_segment;

        public void Initialize(IPAddress address, int port)
        {
            _buffer_recv = new byte[4096];
            _buffer_recv_segment = new(_buffer_recv);

            _endPoint = new IPEndPoint(address, port);

            _socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socket.Connect(_endPoint);

        }

        public void StartMessageLoop()
        {
            //replace this with coroutine
            _ = Task.Run(async () =>
            {
                SocketReceiveMessageFromResult res;
                while (true)
                {
                    res = await _socket.ReceiveMessageFromAsync(_buffer_recv_segment, SocketFlags.None, _endPoint);
                    Console.WriteLine($"Recieved message: " +
                        $"{Encoding.UTF8.GetString(_buffer_recv, 0, res.ReceivedBytes)}");
                }
            }
            );
        }

        public async Task Send(byte[] data)
        {
            var s = new ArraySegment<byte>(data);
            await _socket.SendToAsync(s, SocketFlags.None, _endPoint);
        }

        public void Close()
        {
            _socket.Close();
        }
    }

    class program_server
    { 
        static void Main(string[] args)
        {
            var server = new UDPServer();
            server.Initialize();
            server.StartMessageLoop();
            Console.WriteLine("Server Listening!");

            Console.ReadLine();
        }
    }

    class program_client
    {
        static async Task Main(string[] args)
        {
            var client = new UDPClient();
            client.Initialize(IPAddress.Loopback, UDPServer.PORT);
            client.StartMessageLoop();
            await client.Send(Encoding.UTF8.GetBytes("Hello!"));
            Console.WriteLine("Message sent");

            Console.ReadLine();
        }
    }

    public class UDP : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var server = new UDPServer();
            server.Initialize();
            server.StartMessageLoop();
            Console.WriteLine("Server Listening!");

            Console.ReadLine();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
