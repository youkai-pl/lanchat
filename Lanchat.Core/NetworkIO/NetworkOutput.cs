using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.Network;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core.NetworkIO
{
    /// <summary>
    ///     Sending and receiving data using this class.
    /// </summary>
    internal class NetworkOutput : INetworkOutput
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

        public void SendUserData(object content)
        {
            if (!nodeState.Ready) return;
            SendSystemData(content);
        }

        public void SendSystemData(object content)
        {
            var data = new Dictionary<string, object> {{content.GetType().Name, content}};
            networkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }
    }
}