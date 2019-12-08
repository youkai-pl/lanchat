using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class Host
    {
        // Host constructor
        public Host(int port)
        {
            Events = new HostEvents();
            this.port = port;
            udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        // Properties
        public HostEvents Events { get; set; }

        // Fields
        private readonly UdpClient udpClient;
        private readonly int port;

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
                        Events.OnReceivedBroadcast(paperplane, from.Address);
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
                        try { Process(client); } catch (Exception ex) { Trace.WriteLine("Client connection processing error: " + ex.Message); }
                    }).Start();
                }
            });

            // Host client process
            void Process(Socket client)
            {
                byte[] response;
                int received;
                var ip = IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString());
                Events.OnNodeConnected(ip);

                while (true)
                {
                    // Receive data
                    response = new byte[client.ReceiveBufferSize];
                    received = client.Receive(response);
                    if (received == 0)
                    {
                        Events.OnNodeDisconnected(ip);
                        return;
                    }

                    // Decode recieved data
                    List<byte> respBytesList = new List<byte>(response);

                    // Parse json and get data type
                    JObject data = JObject.Parse(Encoding.UTF8.GetString(respBytesList.ToArray()));
                    var type = data.GetValue("type").ToString();

                    // If handshake
                    if (type == "handshake")
                    {
                        Events.OnReceivedHandshake(data.GetValue("content").ToObject<Handshake>(), ip);
                    }

                    // If key
                    if (type == "key")
                    {
                        Events.OnReceivedKey(data.GetValue("content").ToString(), ip);
                    }

                    // If message
                    if (type == "message")
                    {
                        Events.OnReceivedMessage(data.GetValue("content").ToString(), ip);
                    }

                    // If changed nickname
                    if (type == "nickname")
                    {
                        Events.OnChangedNickname(data.GetValue("content").ToString(), ip);
                    }
                }
            }
        }
    }
}