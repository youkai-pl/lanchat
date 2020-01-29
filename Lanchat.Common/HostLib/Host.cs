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

    internal class Host : IDisposable
    {
        private readonly int port;

        private readonly UdpClient udpClient;

        internal Host(int port)
        {
            Events = new HostEvents();
            this.port = port;
            udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        internal HostEvents Events { get; set; }

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
                    catch (JsonSerializationException)
                    {
                        Trace.WriteLine($"({from.Address}) Paperplane parsing error");
                    }
                }
            });
        }

        internal void StartBroadcast(object self)
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

        internal void StartHost(int port)
        {
            Task.Run(() =>
            {
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    ReceiveTimeout = -1,
                };

                server.Bind(new IPEndPoint(IPAddress.Any, port));
                server.Listen(-1);

                while (true)
                {
                    var socket = server.Accept();
                    var ip = IPAddress.Parse(((IPEndPoint)socket.RemoteEndPoint).Address.ToString());
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                    new Thread(() =>
                    {
                        try
                        {
                            Process(socket, ip);
                        }
                        catch (SocketException)
                        {
                            // Disconnect node on exception
                            Trace.WriteLine($"({ip}) Socket exception. Node will be disconnected");
                            Events.OnNodeDisconnected(ip);
                            socket.Close();
                        }
                    }).Start();
                }
            });

            // Client process
            void Process(Socket socket, IPAddress ip)
            {
                byte[] response;
                int received;

                Events.OnNodeConnected(ip);

                while (true)
                {
                    response = new byte[socket.ReceiveBufferSize];
                    received = socket.Receive(response);

                    if (!socket.IsConnected())
                    {
                        Trace.WriteLine($"({ip}) Socket for {ip} closed");
                        socket.Close();
                        Events.OnNodeDisconnected(ip);
                        break;
                    }

                    try
                    {
                        var respBytesList = new List<byte>(response);
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

                            // Events

                            if (type == "handshake")
                            {
                                Events.OnReceivedHandshake(content.ToObject<Handshake>(), ip);
                            }

                            if (type == "key")
                            {
                                Events.OnReceivedKey(content.ToObject<Key>(), ip);
                            }

                            if (type == "heartbeat")
                            {
                                Events.OnReceivedHeartbeat(ip);
                            }

                            // Data

                            if (type == "message")
                            {
                                Events.OnReceivedMessage(content.ToString(), ip);
                            }

                            if (type == "nickname")
                            {
                                Events.OnChangedNickname(content.ToString(), ip);
                            }

                            if (type == "list")
                            {
                                Events.OnReceivedList(content.ToObject<List<ListItem>>());
                            }

                            // Requests

                            if (type == "request")
                            {
                                // Request type: nickname
                                if (content.ToString() == "nickname")
                                {
                                    Events.OnReceivedRequest("nickname", ip);
                                }
                            }
                        }
                    }
                    catch (DecoderFallbackException)
                    {
                        Trace.WriteLine($"({ip}) Data processing error: utf8 decode gone wrong");
                    }
                    catch (JsonReaderException)
                    {
                        Trace.WriteLine($"({ip}) Data processing error: not vaild json");
                    }
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    udpClient.Dispose();
                }

                disposedValue = true;
            }
        }

        ~Host()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}