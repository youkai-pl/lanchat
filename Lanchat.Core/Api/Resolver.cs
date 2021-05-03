using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Lanchat.Core.Encryption;
using Lanchat.Core.Json;
using Lanchat.Core.Node;

namespace Lanchat.Core.Api
{
    /// <inheritdoc />
    public class Resolver : IResolver
    {
        private readonly IModelEncryption encryption;
        private readonly List<IApiHandler> handlers = new();
        private readonly JsonUtils jsonUtils;
        private readonly INodeInternal node;

        internal Resolver(INodeInternal node)
        {
            this.node = node;
            encryption = node.ModelEncryption;
            jsonUtils = new JsonUtils();
        }

        /// <inheritdoc />
        public void RegisterHandler(IApiHandler apiHandler)
        {
            handlers.Add(apiHandler);
            jsonUtils.KnownModels.Add(apiHandler.HandledType);
        }
        
        /// <inheritdoc />
        public void CallHandler(string item)
        {
            var data = jsonUtils.Deserialize(item);
            var handler = GetHandler(data.GetType());
            if (!CheckPreconditions(handler, data))
            {
                return;
            }

            encryption.DecryptObject(data);
            Trace.WriteLine($"Node {node.Id} received {handler.HandledType.Name}");
            handler.Handle(data);
        }

        private bool CheckPreconditions(IApiHandler handler, object data)
        {
            if (!node.Ready && handler.Privileged == false)
            {
                Trace.WriteLine($"{node.Id} must be ready to handle this type of data.");
                return false;
            }

            if (handler.Disable)
            {
                Trace.WriteLine("Handler disabled");
                return false;
            }

            if (!Validator.TryValidateObject(data, new ValidationContext(data), new List<ValidationResult>()))
            {
                Trace.WriteLine($"Node {node.Id} received invalid json");
                return false;
            }

            return true;
        }

        private IApiHandler GetHandler(Type jsonType)
        {
            return handlers.First(x => x.HandledType == jsonType);
        }
    }
}