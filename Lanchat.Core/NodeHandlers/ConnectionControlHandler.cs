using System.Diagnostics;
using Lanchat.Core.API;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.NodeHandlers
{
    internal class ConnectionControlHandler : ApiHandler<ConnectionControl>
    {
        private readonly INetworkElement networkElement;

        internal ConnectionControlHandler(INetworkElement networkElement)
        {
            this.networkElement = networkElement;
        }

        protected override void Handle(ConnectionControl connectionControl)
        {
            switch (connectionControl.Status)
            {
                case ConnectionControlStatus.RemoteClose:
                    networkElement.Close();
                    break;

                default:
                    Trace.WriteLine("Invalid status received");
                    break;
            }
        }
    }
}