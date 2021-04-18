using Lanchat.Core.Models;

namespace Lanchat.Core
{
    internal class NodeInternals : INodeInternals
    {
        private readonly Node node;

        public NodeInternals(Node node)
        {
            this.node = node;
        }

        public string Nickname
        {
            set => node.Nickname = value;
        }

        public Status Status
        {
            set => node.Status = value;
        }

        public bool Ready
        {
            get => node.Ready;
            set => node.Ready = value;
        }

        public bool IsSession => node.IsSession;

        public bool HandshakeReceived
        {
            get => node.HandshakeReceived;
            set => node.HandshakeReceived = value;
        }

        public void SendHandshake()
        {
            node.SendHandshake();
        }

        public void OnConnected()
        {
            node.OnConnected();
        }

        public void OnCannotConnect()
        {
            node.OnCannotConnect();
        }
    }
}