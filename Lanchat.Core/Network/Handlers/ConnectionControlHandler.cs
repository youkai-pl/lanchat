using Lanchat.Core.Api;
using Lanchat.Core.Network.Models;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.Network.Handlers
{
    internal class ConnectionControlHandler : ApiHandler<ConnectionControl>
    {
        private readonly IHost host;

        public ConnectionControlHandler(IHost host)
        {
            this.host = host;
        }

        protected override void Handle(ConnectionControl connectionControl)
        {
            switch (connectionControl.Status)
            {
                case ConnectionStatus.RemoteDisconnect:
                    host.Close();
                    break;
            }
        }
    }
}