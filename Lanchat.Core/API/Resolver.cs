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
            jsonReader.KnownModels.Add(apiHandler.HandledType);
        }

        internal void HandleJson(string item)
        {
            var data = jsonReader.Deserialize(item);
            var handler = GetHandler(data.GetType());

            if (!nodeState.Ready && handler.Privileged == false)
                throw new InvalidOperationException($"{nodeState.Id} must be ready to handle this type of data.");
            Validator.ValidateObject(data!, new ValidationContext(data), true);
            Trace.WriteLine($"Node {nodeState.Id} received {handler.HandledType.Name}");
            handler.Handle(data);
        }
        
        private IApiHandler GetHandler(Type jsonType)
        {
            var handler = handlers.FirstOrDefault(x => x.HandledType == jsonType);
            if (handler == null)
                throw new ArgumentException($"{nodeState.Id} has no handler for received data.", jsonType.Name);

            return handler;
        }
    }
}