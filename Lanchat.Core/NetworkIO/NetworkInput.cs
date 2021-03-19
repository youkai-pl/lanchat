using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.Models;
using Lanchat.Core.System;

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
            serializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
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
                    var json = JsonSerializer.Deserialize<Dictionary<string, string>>(item, serializerOptions);
                    var jsonType = json.Keys.First();
                    var jsonValue = json.Values.First();
                    var parsedType = Enum.Parse<DataTypes>(jsonType);
                    
                    if (json == null)
                    {
                        Trace.WriteLine($"Node {nodeState.Id} received empty data.");
                        return;
                    }

                    // If node isn't ready ignore every messages except handshake and key info.
                    if (!nodeState.Ready && parsedType != DataTypes.Handshake && parsedType != DataTypes.KeyInfo) return;

                    var assemblyQualifiedName =
                        Assembly.CreateQualifiedName("Lanchat.Core", $"Lanchat.Core.Models.{jsonType}");
                    var type = Type.GetType(assemblyQualifiedName);

                    var data = type == null
                        ? jsonValue
                        : JsonSerializer.Deserialize(jsonValue ?? string.Empty, type, serializerOptions);

                    if (parsedType == DataTypes.NodesList)
                    {
                        data = JsonSerializer.Deserialize(jsonValue ?? string.Empty, typeof(List<string>),
                            serializerOptions);
                    }

                    var handler = ApiHandlers.FirstOrDefault(x => x.HandledDataTypes.Contains(parsedType));
                    if (handler == null)
                    {
                        Trace.WriteLine($"Node {nodeState.Id} received data of unknown type.");
                        return;
                    }

                    Trace.WriteLine($"Node {nodeState.Id} received {parsedType}");

                    if (data == null)
                    {
                        handler.Handle(parsedType);
                    }
                    else if (Validate(data))
                    {
                        handler.Handle(parsedType, data);
                    }
                }

                // Input errors catching.
                catch (Exception ex)
                {
                    if (ex is not JsonException &&
                        ex is not ArgumentNullException &&
                        ex is not NullReferenceException) throw;
                }
        }

        private bool Validate(object data)
        {
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(data, new ValidationContext(data), results, true)) return true;

            foreach (var e in results)
            {
                Trace.WriteLine($"Node {nodeState.Id} received invalid data: {e}");
            }

            return false;
        }
    }
}