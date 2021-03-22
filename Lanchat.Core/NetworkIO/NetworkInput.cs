using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core.NetworkIO
{
    internal class NetworkInput
    {
        private readonly INodeState nodeState;
        private readonly JsonSerializerOptions serializerOptions;
        private readonly Resolver resolver;
        private string buffer;
        private string currentJson;

        internal NetworkInput(INodeState nodeState, Resolver resolver)
        {
            this.nodeState = nodeState;
            serializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
            this.resolver = resolver;
            
            resolver.Models.AddRange(Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.Namespace == "Lanchat.Core.Models"));
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
                    var json = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(item, serializerOptions);
                    var jsonType = json?.Keys.First();
                    var jsonValue = json?.Values.First().ToString();

                    if (jsonType == null || jsonValue == null)
                    {
                        Trace.WriteLine($"Node {nodeState.Id} received empty data.");
                        return;
                    }

                    // If node isn't ready ignore every messages except handshake and key info.
                    if (!nodeState.Ready && jsonType != "Handshake" && jsonType != "KeyInfo") return;

                    Trace.WriteLine($"Node {nodeState.Id} received {jsonType}");
                    resolver.Handle(jsonType, jsonValue);
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