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

        /// <summary>
        ///     Add data handler for specific model type.
        /// </summary>
        /// <param name="apiHandler">ApiHandler object.</param>
        public void RegisterHandler(IApiHandler apiHandler)
        {
            handlers.Add(apiHandler);
        }

        internal void HandleJson(string item)
        {
            var json = DeserializeWrapper(item);
            var handler = GetHandler(json.Keys.First());
            var data = DeserializeData(json.Values.First().ToString(), handler.HandledType);
            if (!nodeState.Ready && handler.Privileged == false)
                throw new InvalidOperationException($"{nodeState.Id} must be ready to handle this type of data.");
            Validator.ValidateObject(data!, new ValidationContext(data), true);
            Trace.WriteLine($"Node {nodeState.Id} received {handler.HandledType.Name}");
            handler.Handle(data);
        }

        private object DeserializeData(string jsonValue, Type type)
        {
            return JsonSerializer.Deserialize(jsonValue, type, serializerOptions);
        }

        private Dictionary<string, JsonElement> DeserializeWrapper(string item)
        {
            return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(item, serializerOptions);
        }

        private IApiHandler GetHandler(string jsonType)
        {
            var handler = handlers.FirstOrDefault(x => x.HandledType.Name == jsonType);
            if (handler == null)
                throw new ArgumentException($"{nodeState.Id} has no handler for received data.", jsonType);

            return handler;
        }
    }
}