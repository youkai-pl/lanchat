using System.Text.Json;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

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

        internal NetworkOutput(INetworkElement networkElement, INodeState nodeState )
        {
            this.networkElement = networkElement;
            this.nodeState = nodeState;
            serializerOptions = CoreConfig.JsonSerializerOptions;
        }

        public void SendUserData(DataTypes dataType, object content = null)
        {
            if (!nodeState.Ready) return;
            SendSystemData(dataType, content);
        }

        public void SendSystemData(DataTypes dataType, object content = null)
        {
            var data = new Wrapper {Type = dataType, Data = content};
            networkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }
    }
}