using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.Network;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core.API
{
    /// <inheritdoc />
    public class NetworkOutput : INetworkOutput
    {
        private readonly INetworkElement networkElement;
        private readonly INodeState nodeState;
        private readonly JsonSerializerOptions serializerOptions;

        internal NetworkOutput(INetworkElement networkElement, INodeState nodeState)
        {
            this.networkElement = networkElement;
            this.nodeState = nodeState;
            serializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
        }

        /// <inheritdoc />
        public void SendData(object content)
        {
            if (!nodeState.Ready) return;
            SendPrivilegedData(content);
        }
        
        /// <inheritdoc />
        public void SendPrivilegedData(object content)
        {
            var data = new Dictionary<string, object> {{content.GetType().Name, content}};
            networkElement.Send(JsonSerializer.Serialize(data, serializerOptions));
        }
    }
}