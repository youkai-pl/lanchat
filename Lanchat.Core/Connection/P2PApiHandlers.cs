using System;
using System.Collections.Generic;
using System.Net;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.Connection
{
    internal class P2PApiHandlers : IApiHandler
    {
        private readonly P2P network;

        internal P2PApiHandlers(P2P network)
        {
            this.network = network;
        }
        
        public IEnumerable<DataTypes> HandledDataTypes { get; } = new[]
        {
            DataTypes.NodesList
        };
        
        public void Handle(DataTypes type, object data)
        {
            var stringList = (List<string>) data;
            var list = new List<IPAddress>();
            
            // Convert strings to ip addresses.
            stringList?.ForEach(x =>
            {
                if (IPAddress.TryParse(x, out var ipAddress)) list.Add(ipAddress);
            });
            
            if (!CoreConfig.AutomaticConnecting) return;
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