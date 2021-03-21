using System;
using System.Collections.Generic;
using System.Net;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.Handlers
{
    internal class P2PApiHandlers : ApiHandler<NodesList>
    {
        private readonly P2P network;
        private readonly IConfig config;

        internal P2PApiHandlers(P2P network, IConfig config)
        {
            this.network = network;
            this.config = config;
        }

        protected override void Handle(NodesList stringList)
        {
            var list = new List<IPAddress>();

            // Convert strings to ip addresses.
            stringList?.ForEach(x =>
            {
                if (IPAddress.TryParse(x, out var ipAddress)) list.Add(ipAddress);
            });

            if (!config.AutomaticConnecting) return;
            list.ForEach(x =>
            {
                try
                {
                    network.Connect(x);
                }
                catch (Exception e)
                {
                    if (e is not ArgumentException) throw;
                }
            });
        }
    }
}