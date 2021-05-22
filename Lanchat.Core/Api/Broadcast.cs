using System.Collections.Generic;
using Lanchat.Core.Network;

namespace Lanchat.Core.Api
{
    internal class Broadcast : IBroadcast
    {
        private readonly List<INodeInternal> nodes;

        internal Broadcast(List<INodeInternal> nodes)
        {
            this.nodes = nodes;
        }

        public void SendMessage(string message)
        {
            nodes.ForEach(x => x.Messaging.SendMessage(message));
        }

        public void SendData(object data)
        {
            nodes.ForEach(x => x.Output.SendData(data));
        }
    }
}