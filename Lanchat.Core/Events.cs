using System;
using System.Net.Sockets;

namespace Lanchat.Core
{
    public sealed class Events
    {
        public event EventHandler<SocketError> ServerError;
        public event EventHandler<SocketError> ClientError;
        public event EventHandler ClientConnected; 
        public event EventHandler ClientDisconnected;
        public event EventHandler<string> MessageReceived;
        
        internal void OnServerError(object sender, SocketError e)
        {
            ServerError?.Invoke(sender, e);
        }

        internal void OnClientError(object sender,SocketError e)
        {
            ClientError?.Invoke(sender, e);
        }

        internal void OnClientConnected(object sender)
        {
            ClientConnected?.Invoke(sender, EventArgs.Empty);
        }

        internal void OnClientDisconnected(object sender)
        {
            ClientDisconnected?.Invoke(this, EventArgs.Empty);
        }

        internal void OnMessageReceived(object sender, string e)
        {
            MessageReceived?.Invoke(sender, e);
        }
    }
}