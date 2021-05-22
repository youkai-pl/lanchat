using Lanchat.Core.Api;
using Lanchat.Core.Identity.Models;

namespace Lanchat.Core.Identity.Handlers
{
    internal class UserStatusUpdateHandler : ApiHandler<UserStatusUpdate>
    {
        private readonly IInternalUser user;

        public UserStatusUpdateHandler(IInternalUser user)
        {
            this.user = user;
        }

        protected override void Handle(UserStatusUpdate userStatus)
        {
            user.UserStatus = userStatus.NewUserStatus;
        }
    }
}