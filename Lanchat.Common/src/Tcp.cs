using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Lanchat.Common.TcpLib
{
    public class Host
    {
        public void Start(int port)
        {
            // Start server
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.ReceiveTimeout = -1;
            server.Bind(new IPEndPoint(IPAddress.Any, port));
            server.Listen(-1);

            while (true)
            {
                Socket client = server.Accept();
                new Thread(() =>
                {
                    try { Process(client); } catch (Exception ex) { Console.WriteLine("Client connection processing error: " + ex.Message); }
                }).Start();
            }

            void Process(Socket client)
            {
                OnHostEvent(new Status("connected", IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString())), EventArgs.Empty);

                byte[] response;
                int received;

                while (true)
                {
                    // Receive message from the server:
                    response = new byte[client.ReceiveBufferSize];
                    received = client.Receive(response);
                    if (received == 0)
                    {
                        OnHostEvent(new Status("disconnected", IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString())), EventArgs.Empty);
                        return;
                    }

                    List<byte> respBytesList = new List<byte>(response);
                    Recieve(Encoding.UTF8.GetString(respBytesList.ToArray()));
                }
            }
        }

        // Handle packet
        void Recieve(string data)
        {
            var parsed = 
        }

        // Host event
        public delegate void HostEventHandler(object o, EventArgs e);

        public event HostEventHandler HostEvent;

        protected virtual void OnHostEvent(object o, EventArgs e)
        {
            HostEvent(o, EventArgs.Empty);
        }

        // Status
        public class Status
        {
            public Status(string type, IPAddress ip)
            {
                Type = type;
                Ip = ip;
            }

            public string Type { get; set; }
            public IPAddress Ip { get; set; }
        }

        // Message
        public class Message
        {
            public Message(string content)
            {
                Content = content;
            }

            public string Content { get; set; }
        }

        // Handshake
        public class Handshake
        {
            public Handshake(string hash)
            {
                Hash = hash;
            }

            public string Hash { get; set; }
        }
    }

    public class Client
    {
        private TcpClient tcpclnt;
        private NetworkStream nwStream;

        public void Connect(IPAddress ip, int port, string self)
        {
            tcpclnt = new TcpClient(ip.ToString(), port);
            nwStream = tcpclnt.GetStream();
            byte[] bytesToSend = Encoding.UTF8.GetBytes(self);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        public void Send(string content)
        {
            byte[] bytesToSend = Encoding.UTF8.GetBytes(content);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }
    }
}