using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.NodeHandlers
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