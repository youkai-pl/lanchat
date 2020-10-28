using Lanchat.Core;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class List
    {
        public static void Execute(P2P network)
        {
            network.Nodes.ForEach(x => Ui.Log.Add($"{x.Nickname} ({x.Endpoint})"));
        }
    }
}