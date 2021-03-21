using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.Handlers
{
    internal class NicknameUpdateHandler : ApiHandler<NicknameUpdate>
    {
        private readonly Node node;
        private readonly IConfig config;

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