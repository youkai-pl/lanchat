using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.General
{
    public class Disconnect : ICommand
    {
        public string[] Aliases { get; } =
        {
            "disconnect",
            "d"
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
            node.Disconnect();
        }
    }
}