using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.Blocking
{
    public class Unblock : ICommand
    {
        public string[] Aliases { get; } =
        {
            "unblock"
        };
        public int ArgsCount => 1;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] args)
        {
            if (!int.TryParse(args[0], out var id))
            {
                Writer.WriteError(Resources.IncorrectCommandUsage);
                return;
            }

            var node = Program.NodesDatabase.GetNodeInfo(id);
            if (node == null)
            {
                Writer.WriteError(Resources.UserNotFound);
                return;
            }

            node.Blocked = false;
            Writer.WriteStatus(string.Format(Resources.Unblocked, $"{node.Nickname}#{node.Id}"));
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}