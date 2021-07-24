using Lanchat.Core.Extensions;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.Commands.Blocking
{
    public class Blocked : ICommand
    {
        public string Alias => "blocked";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            var tabsManager = Program.Window.TabsManager;
            tabsManager.WriteText($"{Resources._BlockedList} {Program.Config.SavedAddresses.Count}");
            Program.Config.SavedAddresses.ForEach(x => tabsManager.WriteText($"{x}"));
        }
    }
}