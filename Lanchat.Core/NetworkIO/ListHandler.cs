using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public class NodesListHandler : IApiHandler
    {
        private readonly Node node;

        public NodesListHandler(Node node)
        {
            this.node = node;
        }
        
        public DataTypes DataType { get; } = DataTypes.NodesList;
        public void Handle(object data)
        {
            var stringList = JsonSerializer.Deserialize<List<string>>((string)data);
            var list = new List<IPAddress>();

            // Convert strings to ip addresses.
            stringList?.ForEach(x =>
            {
                if (IPAddress.TryParse(x, out var ipAddress)) list.Add(ipAddress);
            });
            
            node.OnNodesListReceived(list);
        }
    }
}