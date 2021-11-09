using System;
using System.Net.Sockets;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Identity;
using Lanchat.Core.Network.Models;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.Network
{
    internal class Node : IDisposable, INode, INodeInternal
    {
        private readonly INodesDatabase nodesDatabase;

        public Node(IHost host, INodesDatabase nodesDatabase)
        {
            this.nodesDatabase = nodesDatabase;
            Host = host;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IUser User { get; set; }
        public IHost Host { get; set; }
        public IFileReceiver FileReceiver { get; set; }
        public IFileSender FileSender { get; set; }
        public IMessaging Messaging { get; set; }
        public IOutput Output { get; set; }
        public INodeRsa NodeRsa { get; set; }

        public Guid Id => Host.Id;
        public bool Ready { get; set; }

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;

        public void Disconnect()
        {
            Output.SendPrivilegedData(new ConnectionControl
            {
                Status = ConnectionStatus.RemoteDisconnect
            });
            Dispose();
        }

        public void Start()
        {
            Host.SocketErrored += (s, e) => SocketErrored?.Invoke(s, e);
            Host.DataReceived += Input.OnDataReceived;
            Connection.Initialize();
        }

        public Connection Connection { get; set; }
        public IInput Input { get; set; }
        public IInternalNodeRsa InternalNodeRsa { get; set; }
        public event EventHandler CannotConnect;

        public void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
            nodesDatabase.SaveNodeInfo(Host.Endpoint.Address, new NodeInfo
            {
                IpAddress = Host.Endpoint.Address,
                Id = nodesDatabase.GetSavedNodesCount(),
                Nickname = User.Nickname
            });
        }

        public void OnDisconnected()
        {
            Ready = false;
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        public void OnCannotConnect()
        {
            CannotConnect?.Invoke(this, EventArgs.Empty);
        }
    }
}