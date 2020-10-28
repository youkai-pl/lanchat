using Lanchat.Core;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class List
    {
        public static void Execute()
        {
            Program.Network.Nodes.ForEach(x => Ui.Log.Add($"{x.Nickname} ({x.Endpoint})"));
        }
    }
}