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
                OnHostEvent(new EventObject("connected", IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString())), EventArgs.Empty);

                byte[] response;
                int received;

                while (true)
                {
                    // Receive message from the server:
                    response = new byte[client.ReceiveBufferSize];
                    received = client.Receive(response);
                    if (received == 0)
                    {
                        OnHostEvent(new EventObject("disconnected", IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString())), EventArgs.Empty);
                        return;
                    }

                    List<byte> respBytesList = new List<byte>(response);
                    string message = Encoding.UTF8.GetString(respBytesList.ToArray());
                    OnHostEvent(new EventObject("message", IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString()), message), EventArgs.Empty);
                }
            }
        }

        // Host event
        public delegate void HostEventHandler(EventObject o, EventArgs e);

        public event HostEventHandler HostEvent;

        protected virtual void OnHostEvent(EventObject o, EventArgs e)
        {
            HostEvent(o, EventArgs.Empty);
        }

        // Host event object
        public class EventObject
        {
            public EventObject(string type, IPAddress ip, string content = null)
            {
                Type = type;
                Ip = ip;
                Content = content;
            }

            public string Type { get; set; }
            public IPAddress Ip { get; set; }
            public string Content { get; set; }
        }
    }

    public class Client
    {
        private TcpClient tcpclnt;
        private NetworkStream nwStream;

        public void Connect(IPAddress ip, int port)
        {
            tcpclnt = new TcpClient(ip.ToString(), port);
            nwStream = tcpclnt.GetStream();
        }

        public void Send(string content)
        {
            byte[] bytesToSend = Encoding.UTF8.GetBytes(content);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }
    }
}