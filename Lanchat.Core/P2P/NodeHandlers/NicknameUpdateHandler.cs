using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.P2P.NodeHandlers
{
    internal class NicknameUpdateHandler : ApiHandler<NicknameUpdate>
    {
        private readonly Node node;

        internal NicknameUpdateHandler(Node node)
        {
            this.node = node;
        }

        protected override void Handle(NicknameUpdate newNickname)
        {
            node.Nickname = newNickname.NewNickname;
        }
    }
}