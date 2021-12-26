using Lanchat.Core.Api;
using Lanchat.Core.Filesystem;
using Lanchat.Core.Identity.Models;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.Identity.Handlers
{
    internal class NicknameUpdateHandler : ApiHandler<NicknameUpdate>
    {
        private readonly IInternalUser user;
        private readonly IHost host;
        private readonly INodesDatabase nodesDatabase;

        public NicknameUpdateHandler(IInternalUser user, IHost host, INodesDatabase nodesDatabase)
        {
            this.user = user;
            this.host = host;
            this.nodesDatabase = nodesDatabase;
        }

        protected override void Handle(NicknameUpdate newNickname)
        {
            user.Nickname = newNickname.NewNickname;
            nodesDatabase.GetNodeInfo(host.Endpoint.Address).Nickname = newNickname.NewNickname;
        }
    }
}