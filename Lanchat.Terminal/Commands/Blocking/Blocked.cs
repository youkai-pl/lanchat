using Lanchat.Core.Extensions;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.Blocking
{
    public class Blocked : ICommand
    {
        public string Alias => "blocked";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            Window.Writer.WriteText($"{Resources._BlockedList} {Program.Config.SavedAddresses.Count}");
            Program.Config.SavedAddresses.ForEach(x => Window.Writer.WriteText($"{x}"));
        }
    }
}