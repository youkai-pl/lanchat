using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace Lanchat.Core.Network
{
    // TODO: Refactor
    internal class Server : TcpServer
    {
        private readonly IConfig config;
        private readonly NodesControl nodesControl;
        
        internal Server(IPAddress address, int port, IConfig config, NodesControl nodesControl) : base(address, port)
        {
            this.config = config;
            this.nodesControl = nodesControl;
            OptionDualMode = true;
        }
        
        internal event EventHandler<Node> SessionCreated;

        protected override void OnStarted()
        {
            Trace.WriteLine($"Server listening on {Endpoint.Port}");
            base.OnStarted();
        }

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);

            session.Connected += (_, _) =>
            {
                if (config.BlockedAddresses.Contains(session.Endpoint.Address))
                {
                    Trace.WriteLine($"Connection from {session.Endpoint.Address} blocked");
                    session.Close();
                }
                else
                {
                    var node = new Node(session, config, true);
                    nodesControl.AddNode(node);
                    SessionCreated?.Invoke(this, node);
                    Trace.WriteLine($"Session for {session.Endpoint.Address} created. Session ID: {session.Id}");
                }
            };

            return session;
        }

        protected override void OnError(SocketError error)
        {
            Trace.WriteLine($"Server errored: {error}");
        }
    }
}