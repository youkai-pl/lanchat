using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TcpClient = NetCoreServer.TcpClient;

namespace Lanchat.Core.Network
{
    internal class Client : TcpClient, INetworkElement
    {
        internal Client(IPAddress address, int port) : base(address, port)
        {
        }

        public event EventHandler Disconnected;
        public event EventHandler<string> DataReceived;
        public event EventHandler<SocketError> SocketErrored;
        public bool IsSession { get; } = false;

        public new void SendAsync(string text)
        {
            base.SendAsync(text);
        }

        public void Close()
        {
            DisconnectAsync();
            while (IsConnected) Thread.Yield();
            Dispose();
        }

        protected override void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
            base.OnDisconnected();
        }

        protected override void OnConnected()
        {
            Trace.WriteLine($"Client {Id} connected");
            base.OnConnected();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var message = Encoding.UTF8.GetString(buffer, (int) offset, (int) size);
            DataReceived?.Invoke(this, message);
        }

        protected override void OnError(SocketError error)
        {
            SocketErrored?.Invoke(this, error);
            Trace.WriteLine($"Client {Id} errored: {error}");
        }
    }
}