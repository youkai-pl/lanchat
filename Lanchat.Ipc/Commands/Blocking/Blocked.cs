using System.Linq;

namespace Lanchat.Ipc.Commands.Blocking
{
    public class Blocked : ICommand
    {
        public string Alias => "blocked";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            var blockedNodes = Program.NodesDatabase.SavedNodes.Where(x => x.Blocked).ToList();
            var addresses = blockedNodes.Select(x => x.Id).ToArray();
            Program.IpcSocket.Send(string.Join(";", addresses));
        }
    }
}