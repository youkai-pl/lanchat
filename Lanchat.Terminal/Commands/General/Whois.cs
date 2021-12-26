using Lanchat.Core.Encryption;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.General
{
    public class Whois : ICommand
    {
        public string[] Aliases { get; } =
        {
            "whois",
            "w"
        };
        public int ArgsCount => 1;
        public int ContextArgsCount => 0;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.User.ShortId == args[0]);
            if (node != null)
            {
                Execute(args, node);
            }
            else
            {
                Writer.WriteError(Resources.UserNotFound);
            }
        }

        public void Execute(string[] args, INode node)
        {
            Writer.WriteText(string.Format(Resources.Whois,
                node.User.Nickname,
                node.User.ShortId,
                node.User.UserStatus,
                node.Host.Endpoint.Address,
                node.Host.Endpoint.Port,
                node.Host.Id,
                node.Host.IsSession,
                RsaFingerprint.GetMd5(node.NodeRsa.Rsa.ExportRSAPublicKey())
            ));
        }
    }
}