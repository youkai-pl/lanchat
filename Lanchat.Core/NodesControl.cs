using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lanchat.Core.Models;

namespace Lanchat.Core
{
    internal class NodesControl
    {
        internal List<Node> Nodes { get; }
        private readonly IConfig config;
 
        internal NodesControl(IConfig config)
        {
            this.config = config;
            Nodes = new List<Node>();
        }

        internal void AddNode(Node node)
        {
            Nodes.Add(node);
            node.Connected += OnConnected;
            node.CannotConnect += CloseNode;
            node.Disconnected += CloseNode;
        }
        
        private void CloseNode(object sender, EventArgs e)
        {
            var node = (Node) sender;
            var id = node.Id;
            Nodes.Remove(node);
            node.Connected -= OnConnected;
            node.CannotConnect -= CloseNode;
            node.Disconnected -= CloseNode;
            node.Dispose();
            Trace.WriteLine($"Node {id} disposed");
        }
        
        private void OnConnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            var nodesList = new NodesList();
            nodesList.AddRange(Nodes.Where(x => x.Id != node.Id)
                .Select(x => x.NetworkElement.Endpoint.Address.ToString()));
            node.NetworkOutput.SendData(nodesList);

            if (!config.SavedAddresses.Contains(node.NetworkElement.Endpoint.Address))
                config.SavedAddresses.Add(node.NetworkElement.Endpoint.Address);
        }
    }
}