using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodeHandlers
{
    internal class StatusUpdateHandler : ApiHandler<StatusUpdate>
    {
        private readonly INodeInternal node;

        internal StatusUpdateHandler(INodeInternal node)
        {
            this.node = node;
        }

        protected override void Handle(StatusUpdate status)
        {
            node.Status = status.NewStatus;
        }
    }
}