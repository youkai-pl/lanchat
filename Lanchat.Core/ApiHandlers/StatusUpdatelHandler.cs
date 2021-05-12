using Lanchat.Core.Api;
using Lanchat.Core.Chat.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.ApiHandlers
{
    internal class StatusUpdateHandler : ApiHandler<UserStatusUpdate>
    {
        private readonly INodeInternal node;

        internal StatusUpdateHandler(INodeInternal node)
        {
            this.node = node;
        }

        protected override void Handle(UserStatusUpdate userStatus)
        {
            node.Messaging.UserStatus = userStatus.NewUserStatus;
        }
    }
}