using System;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Network;

namespace Lanchat.Tests.Mock
{
    public class NetworkMock : INetworkElement
    {
        public IPEndPoint Endpoint { get; } = new(IPAddress.Loopback, 1234);
        public Guid Id { get; } = Guid.NewGuid();
        public bool EnableReconnecting { get; set; }
        public bool IsSession { get; } = false;

        public void SendAsync(string text)
        {
            DataReceived?.Invoke(this, text);
        }

        public void Close()
        { }

        public event EventHandler Connected;
        public event EventHandler<bool> Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event EventHandler<string> DataReceived;
    }
}