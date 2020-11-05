using System.Linq;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class PrivateMessage
    {
        public static void Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Ui.Log.Add(Resources.Manual_M);
                return;
            }

            var node = Program.Network.Nodes.Find(x => x.ShortId == args[0]);
            if (node == null)
            {
                Ui.Log.Add(Resources.Info_NotFound);
                return;
            }
            
            node.NetworkOutput.SendPrivateMessage(string.Join(" ", args.Skip(1)));
        }
    }
}