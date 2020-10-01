using System;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Core.Network
{
    public interface INetworkElement
    {
        IPEndPoint Endpoint { get; }
        Guid Id { get; }

        public Io Io { get; }
        void SendMessage(string text);
        bool SendAsync(string text);
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event EventHandler<string> MessageReceived;
    }
}