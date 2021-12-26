using System.Linq;

namespace Lanchat.Ipc.Commands.Blocking
{
    public class Blocked : ICommand
    {
        public string Alias => "blocked";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            foreach (var node in Program.NodesDatabase.SavedNodes.Where(x => x.Blocked).ToList())
            {
                Program.IpcSocket.Send($"{node.Id}-{node.Nickname}-{node.IpAddress};");
            }
        }
    }
}