using System;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class Node
    {
        private readonly INetworkElement networkElement;
        private readonly Output networkOutput;

        public Node(INetworkElement networkElement)
        {
            this.networkElement = networkElement;
            networkOutput = new Output(this);
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

        private void OnSocketErrored(object sender, SocketError e)
        {
            SocketErrored?.Invoke(this, e);
        }
        
        // Network input
        private void OnDataReceived(object sender, string e)
        {
            var data = JsonSerializer.Deserialize<Wrapper>(e);
            switch (data.Type)
            {
                case DataTypes.Message:
                    var message = JsonSerializer.Deserialize<Message>(data.Data.ToString());
                    DataReceived?.Invoke(this, message.Content);
                    break;
                
                case DataTypes.Ping:
                    DataReceived?.Invoke(this, "Ping");
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        // Network output
        public void SendMessage(string content) => networkOutput.SendMessage(content);
        public void SendPing() => networkOutput.SendPing();
        
        // Client / Session methods
        internal bool SendAsync(string text) => networkElement.SendAsync(text);
    }
}