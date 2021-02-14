using System;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Network;

namespace Lanchat.Tests.Mock
{
    public class NetworkElement : INetworkElement
    {
        public IPEndPoint Endpoint { get; }
        public Guid Id { get; }
        public bool EnableReconnecting { get; set; }
        public void SendAsync(string text)
        { }

        public void Close()
        { }

        public event EventHandler Connected;
        public event EventHandler<bool> Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event EventHandler<string> DataReceived;
    }
}