using System.Linq;
using Lanchat.Core;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.Ui;

namespace Lanchat.Terminal.Commands
{
    public static class Disconnect
    {
        public static void Execute(string[] args, P2P network)
        {
            if (args == null || args.Length < 1)
            {
                Prompt.Log.Add(Resources.Manual_Disconnect);
                return;
            }

            var nickname = string.Join(" ", args).Trim();
            var node = network.Nodes.FirstOrDefault(x => x.Nickname.Equals(nickname));
            if (node != null)
            {
                node.Disconnect();
            }
            else
            {
                Prompt.Log.Add("Not found");
            }
        }
    }
}