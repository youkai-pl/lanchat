using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.Handlers
{
    internal class ConnectionControlHandler : ApiHandler<ConnectionControl>
    {
        private readonly Node node;

        internal ConnectionControlHandler(Node node)
        {
            this.node = node;
        }

        protected override void Handle(ConnectionControl connectionControl)
        {
            switch (connectionControl.Status)
            {
                case ConnectionControlStatus.RemoteClose:
                    node.NetworkElement.EnableReconnecting = false;
                    break;
            }
        }
    }
}