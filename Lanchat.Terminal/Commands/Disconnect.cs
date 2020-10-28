using System.Linq;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class Disconnect
    {
        public static void Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Ui.Log.Add(Resources.Manual_Disconnect);
                return;
            }

            var nickname = string.Join(" ", args).Trim();
            var node = Program.Network.Nodes.FirstOrDefault(x => x.Nickname.Equals(nickname));
            if (node != null)
            {
                node.Disconnect();
            }
            else
            {
                Ui.Log.Add("Not found");
            }
        }
    }
}