using Lanchat.Core.Api;
using Lanchat.Core.Chat.Models;

namespace Lanchat.Core.Chat.Handlers
{
    internal class UserStatusUpdateHandler : ApiHandler<UserStatusUpdate>
    {
        private readonly IMessaging messaging;

        public UserStatusUpdateHandler(IMessaging messaging)
        {
            this.messaging = messaging;
        }

        protected override void Handle(UserStatusUpdate userStatus)
        {
            messaging.UserStatus = userStatus.NewUserStatus;
        }
    }
}