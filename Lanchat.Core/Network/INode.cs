using System;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Core.Network
{
    public interface INode
    {
        public Output Output { get; }
        IPEndPoint Endpoint { get; }
        Guid Id { get; }
        bool SendAsync(string text);
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event EventHandler<string> MessageReceived;
    }
}