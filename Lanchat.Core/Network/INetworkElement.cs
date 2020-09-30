using System;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Core.Network
{
    public interface INetworkElement
    {
        bool SendAsync(string text);
        Guid GetId();
        EndPoint GetEndPoint();

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event EventHandler<string> MessageReceived;
    }
}