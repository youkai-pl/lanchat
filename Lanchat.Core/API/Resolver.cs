using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core.API
{
    
    /// <summary>
    ///     Class used to handle received data.
    /// </summary>
    public class Resolver
    {
        private readonly List<IApiHandler> handlers = new();
        private readonly INodeState nodeState;
        private readonly JsonSerializerOptions serializerOptions;

        internal Resolver(INodeState nodeState)
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

        internal void Handle(string item)
        {
            var json = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(item, serializerOptions);
            var jsonType = json?.Keys.First();
            var jsonValue = json?.Values.First().ToString();

            if (jsonType == null || jsonValue == null)
            {
                Trace.WriteLine($"Node {nodeState.Id} received empty data.");
                return;
            }

            var type = handlers.FirstOrDefault(x => x.HandledType.Name == jsonType)?.HandledType;
            if (type == null) throw new ArgumentException($"{nodeState.Id} received data of unknown type.", jsonType);
            var data = JsonSerializer.Deserialize(jsonValue, type, serializerOptions);
            var handler = handlers.FirstOrDefault(x => x.HandledType == type);
            if (handler == null)
                throw new ArgumentException($"{nodeState.Id} has no handler for received data.", jsonType);

            if (!nodeState.Ready && handler.Privileged == false)
                throw new InvalidOperationException($"{nodeState.Id} must be ready to handle this type of data.");

            Validator.ValidateObject(data!, new ValidationContext(data), true);
            Trace.WriteLine($"Node {nodeState.Id} received {jsonType}");
            handler.Handle(data);
        }

        /// <summary>
        ///     Add data handler for specific model type.
        /// </summary>
        /// <param name="apiHandler">ApiHandler object.</param>
        public void RegisterHandler(IApiHandler apiHandler)
        {
            handlers.Add(apiHandler);
        }
    }
}