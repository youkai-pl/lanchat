using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class Blocked
    {
        public static void Execute()
        {
            Ui.Log.Add($"{Resources.Info_BlockedList} {Program.Config.BlockedAddresses.Count}");
            Program.Config.BlockedAddresses.ForEach(x => Ui.Log.Add($"{x}"));
        }
    }
}