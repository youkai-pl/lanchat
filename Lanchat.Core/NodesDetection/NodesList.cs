using System.Collections.Generic;
using System.Net;
using Lanchat.Core.Network;

namespace Lanchat.Core.NodesDetection
{
    public class NodesList
    {
        internal NodesList(INode sender, List<IPAddress> addresses)
        {
            Sender = sender;
            Addresses = addresses;
        }

        public INode Sender {get;}
        public List<IPAddress> Addresses { get; }
    }
}