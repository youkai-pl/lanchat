using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.P2P.NodeHandlers
{
    internal class StatusUpdateHandler : ApiHandler<StatusUpdate>
    {
        private readonly Node node;

        internal StatusUpdateHandler(Node node)
        {
            this.node = node;
        }

        protected override void Handle(StatusUpdate status)
        {
            node.Status = status.NewStatus;
        }
    }
}