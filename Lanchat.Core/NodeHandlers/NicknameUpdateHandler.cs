using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.NodeHandlers
{
    internal class NicknameUpdateHandler : ApiHandler<NicknameUpdate>
    {
        private readonly IConfig config;
        private readonly Node node;

        internal NicknameUpdateHandler(Node node, IConfig config)
        {
            this.node = node;
            this.config = config;
        }

        protected override void Handle(NicknameUpdate newNickname)
        {
            node.Nickname = newNickname.NewNickname.Truncate(config.MaxNicknameLenght);
        }
    }
}