using System;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Tcp;

namespace Lanchat.Tests.Mock
{
    public class NetworkMock : IHost
    {
        public IPEndPoint Endpoint { get; } = new(IPAddress.Loopback, 1234);
        public Guid Id { get; } = Guid.NewGuid();
        public bool IsSession => false;
        public bool Closed { get; private set; }

        public void Send(string text)
        {
            DataReceived?.Invoke(this, text);
        }

        public void Close()
        {
            Closed = true;
        }

        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event EventHandler<string> DataReceived;
    }
}