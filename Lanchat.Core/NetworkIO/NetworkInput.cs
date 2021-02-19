using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    internal class NetworkInput
    {
        public readonly List<IApiHandler> ApiHandlers = new();
        private readonly INodeState nodeState;
        private readonly JsonSerializerOptions serializerOptions;

        private string buffer;
        private string currentJson;

        internal NetworkInput(INodeState nodeState)
        {
            this.nodeState = nodeState;
            serializerOptions = CoreConfig.JsonSerializerOptions;
        }

        internal void ProcessReceivedData(object sender, string dataString)
        {
            buffer += dataString;
            var index = buffer.LastIndexOf("}", StringComparison.Ordinal);
            if (index < 0) return;
            currentJson = buffer.Substring(0, index + 1);
            buffer = buffer.Substring(index + 1);
            
            foreach (var item in currentJson.Replace("}{", "}|{").Split('|'))
                try
                {
                    var json = JsonSerializer.Deserialize<Wrapper>(item, serializerOptions);

                    // If node isn't ready ignore every messages except handshake and key info.
                    if (!nodeState.Ready && json.Type != DataTypes.Handshake && json.Type != DataTypes.KeyInfo) return;

                    // Ignore handshake and key info is node was set as ready before.
                    if (nodeState.Ready && (json.Type == DataTypes.Handshake || json.Type == DataTypes.KeyInfo)) return;


                    var assemblyQualifiedName =
                        Assembly.CreateQualifiedName("Lanchat.Core", $"Lanchat.Core.Models.{json.Type.ToString()}");
                    var type = Type.GetType(assemblyQualifiedName);

                    var data = type == null
                        ? json.Data.ToString()
                        : JsonSerializer.Deserialize(json.Data.ToString(), type, serializerOptions);

                    if (json.Type == DataTypes.NodesList)
                    {
                        data = JsonSerializer.Deserialize(json.Data.ToString(), typeof(List<string>),
                            serializerOptions);
                    }

                    Trace.WriteLine($"Node {nodeState.Id} received {json.Type}");

                    var handler = ApiHandlers.FirstOrDefault(x => x.HandledDataTypes.Contains(json.Type));

                    if (handler != null)
                        handler.Handle(json.Type, data);
                    else
                        Trace.WriteLine($"Node {nodeState.Id} received data of unknown type.");
                }

                // Input errors catching.
                catch (Exception ex)
                {
                    if (ex is not JsonException &&
                        ex is not ArgumentNullException &&
                        ex is not NullReferenceException) throw;
                }
        }
    }
}