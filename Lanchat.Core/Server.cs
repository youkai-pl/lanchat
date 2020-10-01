﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Network;
using NetCoreServer;

namespace Lanchat.Core
{
    public class Server : TcpServer, INetwork
    {
        public Server(IPAddress address, int port) : base(address, port)
        {
            IncomingConnections = new List<INetworkElement>();
        }

        public List<INetworkElement> IncomingConnections { get; }

        public void BroadcastMessage(string message)
        {
            IncomingConnections.ForEach(x => x.Io.SendMessage(message));
        }

        public event EventHandler<SocketError> ServerErrored;
        public event EventHandler<Session> SessionCreated;

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);
            session.Disconnected += SessionOnSessionDisconnected;
            IncomingConnections.Add(session);
            SessionCreated?.Invoke(this, session);
            return session;
        }

        private void SessionOnSessionDisconnected(object sender, EventArgs e)
        {
            var session = (Session) sender;
            IncomingConnections.Remove(session);
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
        }
    }
}