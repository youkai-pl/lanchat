using System;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core
{
    public class Node
    {
        private readonly INetworkElement networkElement;

        public Node(INetworkElement networkElement)
        {
            this.networkElement = networkElement;
            networkElement.Connected += OnConnected;
            networkElement.Disconnected += OnDisconnected;
            networkElement.DataReceived += OnDataReceived;
            networkElement.SocketErrored += OnSocketErrored;
        }

        public Guid Id => networkElement.Id;
        public IPEndPoint Endpoint => networkElement.Endpoint;

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event EventHandler<string> DataReceived;

        // Events forwarding
        private void OnConnected(object sender, EventArgs e)
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        private void OnDataReceived(object sender, string e)
        {
            DataReceived?.Invoke(this, e);
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
            SocketErrored?.Invoke(this, e);
        }
        
        // Network IO
        public void SendMessage(string content)
        {
            var data = new DataWrapper<Message>(new Message {Content = content});
            var json = JsonSerializer.Serialize(data);
            networkElement.SendAsync(json);
        }
    }
}