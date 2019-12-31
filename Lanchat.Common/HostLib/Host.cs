using Lanchat.Common.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lanchat.Common.HostLib
{
    internal static class SocketExtensions
    {
        internal static bool IsConnected(this Socket socket)
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

    internal class Host
    {
        private readonly int port;

        // Fields
        private readonly UdpClient udpClient;

        // Host constructor
        internal Host(int port)
        {
            Events = new HostEvents();
            this.port = port;
            udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        // Properties
        internal HostEvents Events { get; set; }

        // Start broadcast
        internal void Broadcast(object self)
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
        internal void ListenBroadcast()
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
        internal void StartHost(int port)
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
                    var socket = server.Accept();
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    new Thread(() =>
                    {
                        try
                        {
                            Process(socket);
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine("Socket connection processing error: " + ex.Message);
                        }
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
                        break;
                    }

                    try
                    {
                        // Create byte array
                        List<byte> respBytesList = new List<byte>(response);

                        // Decode data
                        var data = Encoding.UTF8.GetString(respBytesList.ToArray());

                        // Parse jsons
                        IList<JObject> buffer = new List<JObject>();
                        JsonTextReader reader = new JsonTextReader(new StringReader(data))
                        {
                            SupportMultipleContent = true
                        };

                        while (true)
                        {
                            if (!reader.Read())
                            {
                                break;
                            }

                            JsonSerializer serializer = new JsonSerializer();
                            JObject packet = serializer.Deserialize<JObject>(reader);

                            buffer.Add(packet);
                        }

                        // Process all parsed jsons from buffer
                        foreach (JObject packet in buffer)
                        {
                            IList<JToken> obj = packet;
                            var type = ((JProperty)obj[0]).Name;
                            var content = ((JProperty)obj[0]).Value;

                            // Type: handshake
                            if (type == "handshake")
                            {
                                Events.OnReceivedHandshake(content.ToObject<Handshake>(), ip);
                            }

                            // Type: key
                            if (type == "key")
                            {
                                Events.OnReceivedKey(content.ToObject<Key>(), ip);
                            }

                            // Type: heartbeat
                            if (type == "heartbeat")
                            {
                                Events.OnReceivedHeartbeat(ip);
                            }

                            // Type: message
                            if (type == "message")
                            {
                                Events.OnReceivedMessage(content.ToString(), ip);
                            }

                            // Type: nickname
                            if (type == "nickname")
                            {
                                Events.OnChangedNickname(content.ToString(), ip);
                            }

                            // Type: request:nickname
                            if (type == "request:nickname")
                            {
                                Events.OnReceivedRequest("nickname", ip);
                            }
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

                Trace.WriteLine($"Socket for {ip} closed");
            }
        }
    }
}