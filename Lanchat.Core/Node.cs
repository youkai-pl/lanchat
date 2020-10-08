using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class Node
    {
        private readonly INetworkElement networkElement;
        private readonly NetworkIO networkIO;

        public Node(INetworkElement networkElement)
        {
            this.networkElement = networkElement;
            networkIO = new NetworkIO(this);
            networkElement.Connected += OnConnected;
            networkElement.Disconnected += OnDisconnected;
            networkElement.DataReceived += OnDataReceived;
            networkElement.SocketErrored += OnSocketErrored;
        }

        // Node properties
        public string Nickname { get; private set; }
        public bool Ready { get; private set; }

        // Network element properties
        public Guid Id => networkElement.Id;
        public IPEndPoint Endpoint => networkElement.Endpoint;

        // Node events
        public event EventHandler<string> MessageReceived;
        public event EventHandler PingReceived;
        internal event EventHandler<List<IPAddress>> NodesListReceived; 

        // Network element events
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;

        private void OnConnected(object sender, EventArgs e)
        {
            networkIO.SendHandshake();

            // Check is connection established successful after timeout
            Task.Delay(5000).ContinueWith(t =>
            {
                if (!Ready)
                {
                    networkElement.Close();
                }
            });
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            if (Ready)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
            SocketErrored?.Invoke(this, e);
        }

        private void OnDataReceived(object sender, string e)
        {
            try
            {
                var data = networkIO.DeserializeInput(e);

                // If node isn't ready ignore every messages except handshake
                if (!Ready && data.Type != DataTypes.Handshake)
                {
                    return;
                }

                switch (data.Type)
                {
                    case DataTypes.Message:
                        var message = JsonSerializer.Deserialize<Message>(data.Data.ToString());
                        MessageReceived?.Invoke(this, message.Content);
                        break;

                    case DataTypes.Ping:
                        PingReceived?.Invoke(this, EventArgs.Empty);
                        break;

                    case DataTypes.Handshake:
                        var handshake = JsonSerializer.Deserialize<Handshake>(data.Data.ToString());
                        Nickname = handshake.Nickname;
                        Ready = true;
                        Connected?.Invoke(this, EventArgs.Empty);
                        break;

                    case DataTypes.NodesList:
                        var nodesList = new List<IPAddress>();
                        var strings = JsonSerializer.Deserialize<List<string>>(data.Data.ToString());

                        strings.ForEach(x =>
                        {
                            if (IPAddress.TryParse(x, out var ip))
                            {
                                nodesList.Add(ip);
                            }
                        });

                        NodesListReceived?.Invoke(this, nodesList);
                        break;

                    default:
                        Debug.WriteLine("Unknown type received");
                        break;
                }
            }

            // Input errors catching
            catch (Exception ex)
            {
                if (ex is JsonException || ex is ArgumentNullException)
                {
                    Debug.WriteLine("Invalid json received");
                }
                else
                {
                    throw;
                }
            }
        }

        // Node output
        public void SendMessage(string content) => networkIO.SendMessage(content);

        public void SendPing() => networkIO.SendPing();

        internal void SendNodesList(List<Node> list) => networkIO.SendNodesList(list);
        
        // Network element methods
        internal bool SendAsync(string text)
        {
            return networkElement.SendAsync(text);
        }
    }
}