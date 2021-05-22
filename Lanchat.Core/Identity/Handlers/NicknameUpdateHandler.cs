using Lanchat.Core.Api;
using Lanchat.Core.Identity.Models;

namespace Lanchat.Core.Identity.Handlers
{
    internal class NicknameUpdateHandler : ApiHandler<NicknameUpdate>
    {
        private readonly IInternalUser user;

        public NicknameUpdateHandler(IInternalUser user)
        {
            this.user = user;
        }

        protected override void Handle(NicknameUpdate newNickname)
        {
            user.Nickname = newNickname.NewNickname;
        }
    }
}