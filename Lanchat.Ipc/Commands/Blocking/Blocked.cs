using System.Linq;

namespace Lanchat.Ipc.Commands.Blocking
{
    public class Blocked : ICommand
    {
        public string Alias => "blocked";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            var nodeListString = string.Join(",", Program.NodesDatabase.SavedNodes.Where(x => x.Blocked).Select(y => y.Id));
            Program.IpcSocket.Send($"{nodeListString};");
        }
    }
}