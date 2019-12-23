using Lanchat.Common.HostLib.Types;
using System;
using System.Diagnostics;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    public partial class Network
    {
        // Create new node
        public void CreateNode(Guid id, int port, IPAddress ip)
        {
            // Create new node with parameters
            var node = new Node(id, port, ip);

            // Create node events handlers
            node.ReadyChanged += OnReadyChanged;
            node.LowHeartbeat += OnLowHeartbeat;

            // Add node to list
            NodeList.Add(node);

            // Create connection with node
            node.CreateConnection();

            // Send handshake to node
            node.Client.SendHandshake(new Handshake(Nickname, PublicKey, Id, HostPort));

            // Log
            Trace.WriteLine("New node created");
            Trace.Indent();
            Trace.WriteLine(node.Ip);
            Trace.WriteLine(node.Port.ToString());
            Trace.Unindent();

            // Ready change event
            void OnReadyChanged(object sender, EventArgs e)
            {
                Trace.WriteLine($"({node.Ip}) ready state changed to {node.Ready}");
                if (node.Ready)
                {
                    Events.OnNodeConnected(node.Ip, node.Nickname);
                }
                else
                {
                    // Place for special event
                }
            }

            // Low heartbeat event
            void OnLowHeartbeat(object sender, EventArgs e)
            {
                Trace.WriteLine($"({ node.Ip}) heartbeat is low, connection closed");
                CloseNode(node);
            }
        }
    }
}