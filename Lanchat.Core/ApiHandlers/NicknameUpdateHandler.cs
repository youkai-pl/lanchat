using Lanchat.Core.Api;
using Lanchat.Core.Network;
using Lanchat.Core.Network.Models;

namespace Lanchat.Core.ApiHandlers
{
    internal class NicknameUpdateHandler : ApiHandler<NicknameUpdate>
    {
        private readonly INodeInternal node;

        internal NicknameUpdateHandler(INodeInternal node)
        {
            this.node = node;
        }

        protected override void Handle(NicknameUpdate newNickname)
        {
            node.Nickname = newNickname.NewNickname;
        }
    }
}