using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodeHandlers
{
    internal class NicknameUpdateHandler : ApiHandler<NicknameUpdate>
    {
        private readonly INodeInternals nodeInternals;

        internal NicknameUpdateHandler(INodeInternals nodeInternals)
        {
            this.nodeInternals = nodeInternals;
        }

        protected override void Handle(NicknameUpdate newNickname)
        {
            nodeInternals.Nickname = newNickname.NewNickname;
        }
    }
}