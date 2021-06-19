using System.Linq;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class PrivateMessage : ICommand
    {
        public string Alias => "m";
        public int ArgsCount => 2;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.User.ShortId == args[0]);
            if (node == null)
            {
                Ui.Log.AddError(Resources._UserNotFound);
                return;
            }

            var message = string.Join(" ", args.Skip(1));
            Ui.Log.AddMessage(message, $"{Program.Config.Nickname} -> {node.User.Nickname}");
            node.Messaging.SendPrivateMessage(message);
        }
    }
}