using Lanchat.Common.Types;
using System;
using System.Diagnostics;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    public partial class Network
    {
        // Create new node
        internal void CreateNode(Guid id, int port, IPAddress ip)
        {
            // Create new node with parameters
            var node = new Node(id, port, ip);

            // Create node events handlers
            node.ReadyChanged += OnStatusChanged;

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
            void OnStatusChanged(object sender, EventArgs e)
            {
                // Node ready
                if (node.State == Status.Ready)
                {
                    Trace.WriteLine($"({node.Ip}) ready");
                    Events.OnNodeConnected(node.Ip, node.Nickname);
                }

                // Node suspended
                else if (node.State == Status.Suspended)
                {
                    Trace.WriteLine($"({node.Ip}) suspended");
                    Events.OnNodeSuspended(node.Ip, node.Nickname);
                }

                // Node resumed
                else if (node.State == Status.Resumed)
                {
                    Trace.WriteLine($"({node.Ip}) resumed");
                    node.State = Status.Ready;
                    Events.OnNodeResumed(node.Ip, node.Nickname);
                }
            }
        }
    }
}