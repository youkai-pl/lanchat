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
        private bool hardDisconnect;
        private bool isReconnecting;
        private int reconnectingCount;

        internal Client(IPAddress address, int port) : base(address, port)
        {
            EnableReconnecting = true;
        }

        public bool EnableReconnecting { get; set; }
        public event EventHandler Connected;
        public event EventHandler<bool> Disconnected;
        public event EventHandler<string> DataReceived;
        public event EventHandler<SocketError> SocketErrored;
        public bool IsSession { get; } = false;

        public new void SendAsync(string text)
        {
            base.SendAsync(text);
        }

        public void Close()
        {
            hardDisconnect = true;
            DisconnectAsync();
            while (IsConnected) Thread.Yield();

            Dispose();
        }

        protected override void OnConnected()
        {
            isReconnecting = false;
            Connected?.Invoke(this, EventArgs.Empty);
            Trace.WriteLine($"Client {Id} connected");
        }

        protected override void OnDisconnected()
        {
            // If reconnecting disabled.
            if (!EnableReconnecting)
            {
                Disconnected?.Invoke(this, true);
                Trace.WriteLine($"Client {Id} disconnected");
                return;
            }

            // Don't invoke event during reconnecting.
            if (!isReconnecting) Disconnected?.Invoke(this, false);

            // Stop if reconnect counter is equal 3 or client disconnected caused by local. 
            if (hardDisconnect || reconnectingCount == 3 || !EnableReconnecting)
            {
                Disconnected?.Invoke(this, true);
                Trace.WriteLine($"Client {Id} disconnected");
                return;
            }

            // Try reconnect.
            Thread.Sleep(1000);
            isReconnecting = true;
            reconnectingCount++;
            Trace.WriteLine($"Client {Id} reconnecting");
            ConnectAsync();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            reconnectingCount = 0;
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