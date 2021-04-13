using Lanchat.Core.Json;
using Lanchat.Core.Network;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core.API
{
    /// <summary>
    ///     Send data other of type not belonging to standard Lanchat.Core set.
    /// </summary>
    public class NetworkOutput
    {
        private readonly INetworkElement networkElement;
        private readonly INodeState nodeState;
        private readonly JsonUtils jsonUtils;

        internal NetworkOutput(INetworkElement networkElement, INodeState nodeState)
        {
            this.networkElement = networkElement;
            this.nodeState = nodeState;
            jsonUtils = new JsonUtils();
        }

        /// <summary>
        ///     Send data.
        /// </summary>
        /// <param name="content">Object to send.</param>
        public void SendData(object content)
        {
            if (!nodeState.Ready) return;
            networkElement.Send(jsonUtils.Serialize(content));
        }

        /// <summary>
        ///     Send the data before marking the node as ready (Handshake, KeyInfo...).
        /// </summary>
        /// <param name="content">Object to send.</param>
        public void SendPrivilegedData(object content)
        {
            networkElement.Send(jsonUtils.Serialize(content));
        }
    }
}