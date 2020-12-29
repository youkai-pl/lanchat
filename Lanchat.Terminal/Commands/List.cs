using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class List
    {
        public static void Execute()
        {
            Ui.Log.Add($"{Resources.Info_ConnectedList} ({Program.Network.Nodes.Count})");
            Program.Network.Nodes.ForEach(x => Ui.Log.Add($"{x.Nickname} ({x.Endpoint})"));
            Ui.Log.Add($"{Resources.Info_DetectedList} ({Program.Network.DetectedNodes.Count})");
            Program.Network.DetectedNodes.ForEach(x => Ui.Log.Add($"{x}"));
        }
    }
}