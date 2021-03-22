using System.Diagnostics;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.NodeHandlers
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
                
                default:
                    Trace.WriteLine("Invalid status received");
                    break;
            }
        }
    }
}