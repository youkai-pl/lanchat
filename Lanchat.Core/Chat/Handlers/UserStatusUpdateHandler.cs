using Lanchat.Core.Api;
using Lanchat.Core.Chat.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.Chat.Handlers
{
    internal class UserStatusUpdateHandler : ApiHandler<UserStatusUpdate>
    {
        private readonly Messaging messaging;

        public UserStatusUpdateHandler(Messaging messaging)
        {
            this.messaging = messaging;
        }

        protected override void Handle(UserStatusUpdate userStatus)
        {
            messaging.UserStatus = userStatus.NewUserStatus;
        }
    }
}