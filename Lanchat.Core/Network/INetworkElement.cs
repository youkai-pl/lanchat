using System;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Core.Network
{
    public interface INetworkElement
    {
        IPEndPoint Endpoint { get; }
        Guid Id { get; }
        bool SendAsync(string text);
        void Close();
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event EventHandler<string> DataReceived;
    }
}