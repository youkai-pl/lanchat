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

namespace Lanchat.Common.NetworkLib
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

        public void Dispose()
        {
            udpClient.Dispose();
        }

        internal void ListenBroadcast()
        {
            Task.Run(() =>
            {
                Trace.WriteLine("[HOST] UDP listen started");
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
                        Trace.WriteLine($"[HOST] Paperplane parsing error ({from.Address})");
                    }
                }
            });
        }

        internal void StartBroadcast(object self)
        {
            Task.Run(() =>
              {
                  Trace.WriteLine("[HOST] UDP broadcast started");
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

                Trace.WriteLine("[HOST] TCP host started");
                while (true)
                {
                    var socket = server.Accept();
                    var ip = IPAddress.Parse(((IPEndPoint)socket.RemoteEndPoint).Address.ToString());
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    Events.OnNodeConnected(socket, ip);
                }
            });
        }
    }
}