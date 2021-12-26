using Lanchat.Core.Encryption;

namespace Lanchat.Ipc.Commands.Blocking
{
    public class Whois : ICommand
    {
        public string Alias => "whois";
        public int ArgsCount => 0;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.User.ShortId == args[0]);
            if (node == null)
            {
                Program.IpcSocket.SendError(Error.node_not_found);
                return;
            }

            var nodeInfoString = string.Join(",",
                node.User.Nickname,
                node.User.ShortId,
                node.User.UserStatus,
                node.Host.Endpoint.Address,
                node.Host.Endpoint.Port,
                node.Host.Id,
                node.Host.IsSession,
                RsaFingerprint.GetMd5(node.NodeRsa.Rsa.ExportRSAPublicKey()));

            Program.IpcSocket.Send($"{nodeInfoString};");
        }
    }
}