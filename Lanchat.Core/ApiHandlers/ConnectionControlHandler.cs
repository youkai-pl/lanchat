using System.Diagnostics;
using Lanchat.Core.Api;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.ApiHandlers
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