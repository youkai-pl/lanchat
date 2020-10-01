using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;

namespace Lanchat.Core.Network
{
    public class Session : TcpSession, INetworkElement
    {
        public Session(TcpServer server) : base(server)
        {
            Node = new Node(this);
        }

        public Node Node { get; }
        public IPEndPoint Endpoint => (IPEndPoint) Socket.RemoteEndPoint;
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<string> MessageReceived;
        public event EventHandler<SocketError> SocketErrored;

        protected override void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var message = Encoding.UTF8.GetString(buffer, (int) offset, (int) size);
            MessageReceived?.Invoke(this, message);
        }

        protected override void OnError(SocketError error)
        {
            SocketErrored?.Invoke(this, error);
        }
    }
}