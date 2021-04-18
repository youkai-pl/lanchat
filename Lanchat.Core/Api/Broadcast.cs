using System.Collections.Generic;
using Lanchat.Core.Node;

namespace Lanchat.Core.Api
{
    /// <summary>
    ///     Send data to all nodes.
    /// </summary>
    public class Broadcast
    {
        private readonly List<NodeImplementation> nodes;

        internal Broadcast(List<NodeImplementation> nodes)
        {
            this.nodes = nodes;
        }

        /// <summary>
        ///     Broadcast message.
        /// </summary>
        /// <param name="message">Message content.</param>
        public void SendMessage(string message)
        {
            nodes.ForEach(x => x.Messaging.SendMessage(message));
        }

        /// <summary>
        ///     Broadcast data.
        /// </summary>
        /// <param name="data"></param>
        public void SendData(object data)
        {
            nodes.ForEach(x => x.Output.SendData(data));
        }
    }
}