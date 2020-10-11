using Lanchat.Core;
using Lanchat.Terminal.Ui;

namespace Lanchat.Terminal.Commands
{
    public static class List
    {
        public static void Execute(P2P network)
        {
            network.Nodes.ForEach(x => Prompt.Log.Add($"{x.Nickname} ({x.Endpoint})"));
        }
    }
}