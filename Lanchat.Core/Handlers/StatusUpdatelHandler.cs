using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.Handlers
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