using System;
using System.Collections.Generic;
using System.Net;
using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.P2PHandlers
{
    internal class NodesListHandler : ApiHandler<NodesList>
    {
        private readonly IConfig config;
        private readonly P2P p2P;

        internal NodesListHandler(P2P p2P, IConfig config)
        {
            this.p2P = p2P;
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
                    p2P.Connect(x);
                }
                catch (Exception e)
                {
                    if (e is not ArgumentException) throw;
                }
            });
        }
    }
}