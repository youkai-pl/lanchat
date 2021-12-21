using System.Linq;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.Blocking
{
    public class Blocked : ICommand
    {
        public string[] Aliases { get; } =
        {
            "blocked"
        };
        public int ArgsCount => 0;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] _)
        {
            var blockedNodes = Program.NodesDatabase.SavedNodes.Where(x => x.Blocked).ToList();
            Writer.WriteText($"{Resources.BlockedList}:");
            blockedNodes.ForEach(x => Writer.WriteText($"{x.Nickname}#{x.Id} - {x.IpAddress}"));
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}