using Lanchat.Common.HostLib.Types;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
                    ReceiveTimeout = -1,
                };

                server.Bind(new IPEndPoint(IPAddress.Any, port));
                server.Listen(-1);

                // Start listening
                while (true)
                {
                    Socket socket = server.Accept();
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                    new Thread(() =>
                    {
                        try { Process(socket); } catch (Exception ex) { Trace.WriteLine("Socket connection processing error: " + ex.Message); }
                    }).Start();
                }
            });

            // Host client process
            void Process(Socket socket)
            {
                byte[] response;
                int received;
                var ip = IPAddress.Parse(((IPEndPoint)socket.RemoteEndPoint).Address.ToString());
                Events.OnNodeConnected(ip);

                while (true)
                {
                    // Rceive data
                    response = new byte[socket.ReceiveBufferSize];
                    received = socket.Receive(response);

                    // Check connection
                    if (!socket.IsConnected())
                    {
                        socket.Close();
                        Events.OnNodeDisconnected(ip);
                        return;
                    }

                    try
                    {
                        // Decode recieved data
                        List<byte> respBytesList = new List<byte>(response);

                        // Parse json and get data type
                        IList<JToken> obj = JObject.Parse(Encoding.UTF8.GetString(respBytesList.ToArray()));
                        var type = ((JProperty)obj[0]).Name;
                        var content = ((JProperty)obj[0]).Value;
                        Trace.WriteLine(type);

                        // If handshake
                        if (type == "handshake")
                        {
                            Events.OnReceivedHandshake(content.ToObject<Handshake>(), ip);
                        }

                        // If key
                        if (type == "key")
                        {
                            Events.OnReceivedKey(content.ToObject<Key>(), ip);
                        }

                        // If message
                        if (type == "message")
                        {
                            Events.OnReceivedMessage(content.ToString(), ip);
                        }

                        // If changed nickname
                        if (type == "nickname")
                        {
                            Events.OnChangedNickname(content.ToString(), ip);
                        }
                    }
                    catch (Exception e)
                    {
                        // Handle decoder exception
                        if (e is DecoderFallbackException)
                        {
                            Trace.WriteLine("Data processing error: utf8 decode gone wrong");
                            Trace.WriteLine($"Sender: {ip}");
                        }

                        // Handle json parse exception
                        else if (e is JsonReaderException)
                        {
                            Trace.WriteLine("Data processing error: not vaild json");
                            Trace.WriteLine($"Sender: {ip}");
                        }

                        // Handle other exceptions
                        else
                        {
                            Trace.WriteLine($"Data processing error: {e.GetType()}");
                            Trace.WriteLine($"Sender: {ip}");
                        }
                    }
                }
            }
        }
    }


    public static class SocketExtensions
    {
        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }
}