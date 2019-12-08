using Lanchat.Common.NetworkLib;
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
    internal class Host
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
                        OnReceivedBroadcast(paperplane, from.Address);
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
                OnNodeConnected(ip);

                while (true)
                {
                    // Receive data
                    response = new byte[client.ReceiveBufferSize];
                    received = client.Receive(response);
                    if (received == 0)
                    {
                        OnNodeDisconnected(ip);
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
                        OnReceivedHandshake(data.GetValue("content").ToObject<Handshake>(), ip);
                    }

                    // If key
                    if (type == "key")
                    {
                        OnReceivedKey(data.GetValue("content").ToString(), ip);
                    }

                    // If message
                    if (type == "message")
                    {
                        OnReceivedMessage(data.GetValue("content").ToString(), ip);
                    }

                    // If changed nickname
                    if (type == "nickname")
                    {
                        OnChangedNickname(data.GetValue("content").ToString(), ip);
                    }
                }
            }
        }

        // Recieved broadcast event
        public event EventHandler<RecievedBroadcastEventArgs> RecievedBroadcast;

        protected virtual void OnReceivedBroadcast(Paperplane sender, IPAddress senderIP)
        {
            RecievedBroadcast(this, new RecievedBroadcastEventArgs()
            {
                Sender = sender,
                SenderIP = senderIP
            });
        }

        // Node connected event
        public event EventHandler<NodeConnectionStatusEvent> NodeConnected;

        protected virtual void OnNodeConnected(IPAddress nodeIP)
        {
            NodeConnected(this, new NodeConnectionStatusEvent()
            {
                NodeIP = nodeIP
            });
        }

        // Node connected event
        public event EventHandler<NodeConnectionStatusEvent> NodeDisconnected;

        protected virtual void OnNodeDisconnected(IPAddress nodeIP)
        {
            NodeDisconnected(this, new NodeConnectionStatusEvent()
            {
                NodeIP = nodeIP
            });
        }

        // Recieved handshake event
        public event EventHandler<RecievedHandshakeEventArgs> ReceivedHandshake;

        protected virtual void OnReceivedHandshake(Handshake handshake, IPAddress senderIP)
        {
            ReceivedHandshake(this, new RecievedHandshakeEventArgs()
            {
                NodeHandshake = handshake,
                SenderIP = senderIP
            });
        }

        // Recieved symetric key event
        public event EventHandler<RecievedKeyEventArgs> ReceivedKey;
        protected virtual void OnReceivedKey(string encryptedKey, IPAddress senderIP)
        {
            ReceivedKey(this, new RecievedKeyEventArgs()
            {
                Key = encryptedKey,
                SenderIP = senderIP
            });
        }

        // Recieved message event
        public event EventHandler<ReceivedMessageEventArgs> RecievedMessage;

        protected virtual void OnReceivedMessage(string content, IPAddress senderIP)
        {
            RecievedMessage(this, new ReceivedMessageEventArgs()
            {
                Content = content,
                SenderIP = senderIP
            });
        }

        // Changed nickname event
        public event EventHandler<ChangedNicknameEventArgs> ChangedNickname;

        protected virtual void OnChangedNickname(string newNickname, IPAddress senderIP)
        {
            ChangedNickname(this, new ChangedNicknameEventArgs()
            {
                NewNickname = newNickname,
                SenderIP = senderIP
            });
        }
    }
}