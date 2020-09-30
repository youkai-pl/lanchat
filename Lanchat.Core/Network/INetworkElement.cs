using System;
using System.Net.Sockets;

namespace Lanchat.Core.Network
{
    public interface INetworkElement
    {
        bool SendAsync(string text);
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
    }
}