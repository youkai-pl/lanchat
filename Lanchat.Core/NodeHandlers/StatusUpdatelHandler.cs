using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodeHandlers
{
    internal class StatusUpdateHandler : ApiHandler<StatusUpdate>
    {
        private readonly INodeInternals nodeInternals;

        internal StatusUpdateHandler(INodeInternals nodeInternals)
        {
            this.nodeInternals = nodeInternals;
        }

        protected override void Handle(StatusUpdate status)
        {
            nodeInternals.Status = status.NewStatus;
        }
    }
}