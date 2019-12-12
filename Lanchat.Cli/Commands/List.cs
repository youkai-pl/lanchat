using Lanchat.Cli.Ui;

namespace Lanchat.Cli.Commands
{
    public partial class Command
    {
        public void List()
        {
            Prompt.Out($"Connected peers: {program.Network.NodeList.Count}");
            foreach (var item in program.Network.NodeList)
            {
                Prompt.Out($"{item.Nickname} ({item.Ip})");
            }
        }
    }
}
