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

            apiHandlers.Add(node);
            apiHandlers.Add(node.Messaging);
            apiHandlers.Add(node.Echo);
            apiHandlers.Add(node.FileTransferHandler);
            apiHandlers.Add(node.FileReceiver);
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

                    // If node isn't ready ignore every messages except handshake and key info.
                    if (!node.Ready && json.Type != DataTypes.Handshake && json.Type != DataTypes.KeyInfo) return;

                    // Ignore handshake and key info is node was set as ready before.
                    if (node.Ready && (json.Type == DataTypes.Handshake || json.Type == DataTypes.KeyInfo)) return;

                    Trace.WriteLine($"Node {node.Id} received {json.Type}");

                    var handler = apiHandlers.FirstOrDefault(x => x.HandledDataTypes.Contains(json.Type));
                    
                    if (handler != null)
                    {
                        handler.Handle(json.Type, json.Data.ToString());
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