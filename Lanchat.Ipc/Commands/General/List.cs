using System.Linq;

namespace Lanchat.Ipc.Commands.Blocking
{
    public class List : ICommand
    {
        public string Alias => "list";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            var nodeListString = string.Join(",", Program.Network.Nodes.Select(x => x.User.ShortId));
            Program.IpcSocket.Send($"{nodeListString};");
        }
    }
}