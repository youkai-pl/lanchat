using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class NetworkInput
    {
        private readonly Node node;
        private readonly JsonSerializerOptions serializerOptions;

        internal NetworkInput(Node node)
        {
            this.node = node;
            serializerOptions = CoreConfig.JsonSerializerOptions;
        }

        /// <summary>
        ///     Message received.
        /// </summary>
        public event EventHandler<string> MessageReceived;

        /// <summary>
        ///     Ping received.
        /// </summary>
        public event EventHandler PingReceived;

        internal event EventHandler<Handshake> HandshakeReceived;
        internal event EventHandler<KeyInfo> KeyInfoReceived;
        internal event EventHandler<List<IPAddress>> NodesListReceived;

        internal void ProcessReceivedData(object sender, string json)
        {
            try
            {
                var data = JsonSerializer.Deserialize<Wrapper>(json, serializerOptions);

                // If node isn't ready ignore every messages except handshake
                if (!node.Ready && data.Type != DataTypes.Handshake && data.Type != DataTypes.KeyInfo)
                {
                    return;
                }

                switch (data.Type)
                {
                    case DataTypes.Message:
                        var message = JsonSerializer.Deserialize<Message>(data.Data.ToString());
                        MessageReceived?.Invoke(this, node.Encryption.Decrypt(message.Content));
                        break;

                    case DataTypes.Ping:
                        PingReceived?.Invoke(this, EventArgs.Empty);
                        break;

                    case DataTypes.Handshake:
                        var handshake = JsonSerializer.Deserialize<Handshake>(data.Data.ToString());
                        HandshakeReceived?.Invoke(this, handshake);
                        break;

                    case DataTypes.KeyInfo:
                        var keyInfo = JsonSerializer.Deserialize<KeyInfo>(data.Data.ToString());
                        KeyInfoReceived?.Invoke(this, keyInfo);
                        break;

                    case DataTypes.NodesList:
                        var stringList = JsonSerializer.Deserialize<List<string>>(data.Data.ToString());
                        var list = new List<IPAddress>();

                        // Convert strings to ip addresses
                        stringList.ForEach(x =>
                        {
                            if (IPAddress.TryParse(x, out var ipAddress))
                            {
                                list.Add(ipAddress);
                            }
                        });

                        NodesListReceived?.Invoke(this, list);
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
    }
}