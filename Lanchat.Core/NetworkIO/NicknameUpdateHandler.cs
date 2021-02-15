using Lanchat.Core.Extensions;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public class NicknameUpdateHandler : IApiHandler
    {
        private readonly Node node;

        public NicknameUpdateHandler(Node node)
        {
            this.node = node;
        }
        public DataTypes DataType { get; } = DataTypes.NicknameUpdate;
        public void Handle(object data)
        {
            var nickname = (string) data;;
            node.Nickname = nickname.Truncate(CoreConfig.MaxNicknameLenght);
        }
    }
}