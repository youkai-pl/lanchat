using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TcpClient = NetCoreServer.TcpClient;

namespace Lanchat.Core.Network
{
    public class Client : TcpClient, INetworkElement
    {
        private bool stop;

        public Client(string address, int port) : base(address, port)
        {
            Io = new Io(this);
        }

        public void SendMessage(string text)
        {
            Io.SendMessage(text);
        }

        public Io Io { get; }

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<string> MessageReceived;
        public event EventHandler<SocketError> SocketErrored;

        public void DisconnectAndStop()
        {
            stop = true;
            DisconnectAsync();
            while (IsConnected)
            {
                Thread.Yield();
            }
        }

        protected override void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);

            // Try reconnect after while
            Thread.Sleep(1000);
            if (!stop)
            {
                ConnectAsync();
            }
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