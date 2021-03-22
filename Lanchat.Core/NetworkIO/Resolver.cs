using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.Extensions;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core.NetworkIO
{
    internal class Resolver
    {
        private readonly INodeState nodeState;
        internal readonly List<IApiHandler> Handlers = new();
        internal readonly List<Type> Models = new();
        private readonly JsonSerializerOptions serializerOptions;

        public Resolver(INodeState nodeState)
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

        internal void Handle(string jsonType, string jsonValue)
        {
            var type = Models.FirstOrDefault(x => x.Name == jsonType);
            
            if (type == null)
            {
                Trace.WriteLine("Received data of unknown type.");
                return;
            }
            
            var data = JsonSerializer.Deserialize(jsonValue, type, serializerOptions);
            var handler = Handlers.FirstOrDefault(x => x.HandledType == type);
            if (handler == null)
            {
                Trace.WriteLine("No handler for received data.");
                return;
            }

            if (!nodeState.Ready && handler.Privileged == false)
            {
                Trace.WriteLine("Node isn't ready.");
                return;
            }
            
            if (!ModelValidator.Validate(data))
            {
                Trace.WriteLine("Received invalid data.");
                return;
            }

            handler.Handle(data);
        }
    }
}