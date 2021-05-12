using Lanchat.Core.Api;
using Lanchat.Core.Chat.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.Chat.Handlers
{
    internal class UserStatusUpdateHandler : ApiHandler<UserStatusUpdate>
    {
        private readonly INodeInternal node;

        internal UserStatusUpdateHandler(INodeInternal node)
        {
            this.node = node;
        }

        protected override void Handle(UserStatusUpdate userStatus)
        {
            node.Messaging.UserStatus = userStatus.NewUserStatus;
        }
    }
}