using System;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;

namespace Lanchat.Core.Network
{
    public class Session : TcpSession, INetworkElement
    {
        public Session(TcpServer server) : base(server)
        {
            NetworkOutput = new NetworkOutput(this);
        }

        public NetworkOutput NetworkOutput { get; }
        public event EventHandler SessionConnected;
        public event EventHandler SessionDisconnected;
        public event EventHandler<string> MessageReceived;
        public event EventHandler<SocketError> SessionErrored;

        protected override void OnConnected()
        {
            SessionConnected?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnDisconnected()
        {
            SessionDisconnected?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var message = Encoding.UTF8.GetString(buffer, (int) offset, (int) size);
            MessageReceived?.Invoke(this, message);
        }

        protected override void OnError(SocketError error)
        {
            SessionErrored?.Invoke(this, error);
        }
    }
}