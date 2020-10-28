using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class Blocked
    {
        public static void Execute()
        {
            Ui.Log.Add($"Blocked nodes: {Program.Config.BlockedAddresses.Count}");
            Program.Config.BlockedAddresses.ForEach(x => Ui.Log.Add($"{x}"));
        }
    }
}