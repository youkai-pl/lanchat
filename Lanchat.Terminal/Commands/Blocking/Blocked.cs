using Lanchat.Core.Extensions;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.Blocking
{
    public class Blocked : ICommand
    {
        public string Alias => "blocked";
        public int ArgsCount => 0;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] _)
        {
            Writer.WriteText($"{Resources._BlockedList} {Program.Config.SavedAddresses.Count}");
            Program.Config.SavedAddresses.ForEach(x => Writer.WriteText($"{x}"));
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}