using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Network;
using NetCoreServer;

namespace Lanchat.Core.TransportLayer
{
    internal class Server : TcpServer
    {
        private readonly NodesControl nodesControl;
        private readonly AddressChecker addressChecker;

        internal Server(IPAddress address, int port, NodesControl nodesControl, AddressChecker addressChecker) : base(address, port)
        {
            this.nodesControl = nodesControl;
            this.addressChecker = addressChecker;
            OptionDualMode = true;
        }

        protected override void OnStarted()
        {
            Trace.WriteLine($"Server listening on {Endpoint.Port}");
            base.OnStarted();
        }

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);
            session.Connected += SessionOnConnected;
            return session;
        }

        private void SessionOnConnected(object sender, EventArgs e)
        {
            var session = (Session)sender;

            try
            {
                addressChecker.CheckAddress(session.Endpoint.Address);
                nodesControl.CreateNode(session);
            }
            catch (ArgumentException)
            {
                session.Close();
            }
        }

        protected override void OnError(SocketError error)
        {
            Trace.WriteLine($"Server errored: {error}");
        }
    }
}