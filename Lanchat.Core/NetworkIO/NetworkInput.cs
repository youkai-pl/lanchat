using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    internal class NetworkInput
    {
        private readonly Node node;
        private readonly List<IApiHandler> apiHandlers = new();
        private readonly JsonSerializerOptions serializerOptions;

        private string buffer = string.Empty;

        internal NetworkInput(Node node)
        {
            this.node = node;
            serializerOptions = CoreConfig.JsonSerializerOptions;

            apiHandlers.Add(new MessageHandler(node.Messaging));
            apiHandlers.Add(new HandshakeHandler(node));
            apiHandlers.Add(new KeyInfoHandler(node));
            apiHandlers.Add(new NodesListHandler(node));
            apiHandlers.Add(new NicknameUpdateHandler(node));
            apiHandlers.Add(new GoodbyeHandler(node.NetworkElement));
            apiHandlers.Add(new StatusUpdateHandler(node));
            apiHandlers.Add(new PingHandler(node.Echo));
            apiHandlers.Add(new PongHandler(node.Echo));
            apiHandlers.Add(new FilePartHandler(node.FileReceiver));
            apiHandlers.Add(new FileExchangeRequestHandler(node.FileTransferHandler));
        }
        
        internal void ProcessReceivedData(object sender, string dataString)
        {
            // TODO: MEMORY LEAK!!!
            if (dataString.StartsWith("{") && dataString.EndsWith("}"))
            {
                buffer = dataString;
            }
            else
            {
                buffer += dataString;
                if (!(buffer.StartsWith("{") && buffer.EndsWith("}"))) return;
            }

            foreach (var item in buffer.Replace("}{", "}|{").Split('|'))
                try
                {
                    var json = JsonSerializer.Deserialize<Wrapper>(item, serializerOptions);
                    var content = json.Data?.ToString();

                    // If node isn't ready ignore every messages except handshake and key info.
                    if (!node.Ready && json.Type != DataTypes.Handshake && json.Type != DataTypes.KeyInfo) return;

                    // Ignore handshake and key info is node was set as ready before.
                    if (node.Ready && (json.Type == DataTypes.Handshake || json.Type == DataTypes.KeyInfo)) return;

                    Trace.WriteLine($"Node {node.Id} received {json.Type}");

                    var handler = apiHandlers.FirstOrDefault(x => x.DataType == json.Type);
                    
                    if (handler != null)
                    {
                        handler.Handle(content);
                    }
                    else
                    {
                        Trace.WriteLine($"Node {node.Id} received data of unknown type.");
                    }
                }

                // Input errors catching.
                catch (Exception ex)
                {
                    if (ex is not JsonException &&
                        ex is not ArgumentNullException &&
                        ex is not NullReferenceException) throw;
                }

            buffer = string.Empty;
        }
    }
}