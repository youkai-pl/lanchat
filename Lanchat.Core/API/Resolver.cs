using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
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
        private readonly JsonReader jsonReader;
        internal Resolver(INodeState nodeState)
        {
            this.nodeState = nodeState;
            jsonReader = new JsonReader();
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
            var json = jsonReader.DeserializeWrapper(item);
            var handler = GetHandler(json.Keys.First());
            var data = jsonReader.DeserializeData(json.Values.First().ToString(), handler.HandledType);
            if (!nodeState.Ready && handler.Privileged == false)
                throw new InvalidOperationException($"{nodeState.Id} must be ready to handle this type of data.");
            Validator.ValidateObject(data!, new ValidationContext(data), true);
            Trace.WriteLine($"Node {nodeState.Id} received {handler.HandledType.Name}");
            handler.Handle(data);
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