using System;
using System.Net;
using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.ApiHandlers
{
    internal class NodesListHandler : ApiHandler<NodesList>
    {
        private readonly P2P network;

        internal NodesListHandler(P2P network)
        {
            this.network = network;
        }

        protected override void Handle(NodesList nodesList)
        {
            if (!network.Config.ConnectToReceivedList)
            {
                return;
            }

            nodesList.RemoveAll(x => x.Equals(IPAddress.Loopback));
            nodesList.ForEach(x =>
            {
                try
                {
                    network.Connect(x).ConfigureAwait(false);
                }
                catch (ArgumentException)
                { }
            });
        }
    }
}