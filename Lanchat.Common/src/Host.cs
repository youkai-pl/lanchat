using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lanchat.Common.HostLib
{
    class Host
    {

        private readonly UdpClient udpClient;
        private readonly int port;

        // Host constructor
        public Host(int port)
        {
            this.port = port;
            udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        // Start broadcast
        public void Broadcast(object self)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(self));
                    udpClient.Send(data, data.Length, "255.255.255.255", port);
                    Thread.Sleep(1000);
                }
            });
        }

        // Listen other hosts broadcasts
        public void ListenBroadcast()
        {
            Task.Run(() =>
            {
                var from = new IPEndPoint(0, 0);
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);

                    // Try parse
                    try
                    {
                        var paperplane = JsonConvert.DeserializeObject<Paperplane>(Encoding.UTF8.GetString(recvBuffer));
                        RecievedBroadcast(paperplane, from.Address, EventArgs.Empty);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine("Paperplane parsing error: " + e.Message);
                    }
                }
            });
        }

        // Start host
        public void StartHost(int port)
        {
            Task.Run(() =>
            {
                // Create server
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    ReceiveTimeout = -1
                };
                server.Bind(new IPEndPoint(IPAddress.Any, port));
                server.Listen(-1);

                // Start listening
                while (true)
                {
                    Socket client = server.Accept();
                    new Thread(() =>
                    {
                        try { Process(client); } catch (Exception ex) { Console.WriteLine("Client connection processing error: " + ex.Message); }
                    }).Start();
                }
            });

            // Host client process
            void Process(Socket client)
            {
                Trace.WriteLine("New connection on host");

                byte[] response;
                int received;
                var ip = IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString());

                while (true)
                {
                    // Receive data
                    response = new byte[client.ReceiveBufferSize];
                    received = client.Receive(response);
                    if (received == 0)
                    {
                        // Handle disconnect here
                        return;
                    }

                    // Decode recieved data 
                    List<byte> respBytesList = new List<byte>(response);
                    var data = Encoding.UTF8.GetString(respBytesList.ToArray());

                    // Try parse data as handshake
                    try
                    {
                        var handshake = JsonConvert.DeserializeObject<Handshake>(data);
                        OnRecievedHandshake(handshake, ip, EventArgs.Empty);
                    }
                    catch
                    {
                        Trace.WriteLine(data);
                    }
                }
            }
        }

        // Host events
        public delegate void HostEventHandler(params object[] arguments);

        // Handshake recievie event
        public event HostEventHandler RecievedHandshake;
        protected virtual void OnRecievedHandshake(Handshake handshake, IPAddress ip, EventArgs e)
        {
            RecievedHandshake(handshake, ip, e);
        }

        public event HostEventHandler RecievedBroadcast;

        // Recieved broadcast
        protected virtual void OnRecievedBroadcast(Paperplane paperplane, IPAddress sender, EventArgs e)
        {
            RecievedBroadcast(paperplane, sender, EventArgs.Empty);
        }
    }
}
