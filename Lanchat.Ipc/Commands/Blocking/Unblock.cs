using System.Net;

namespace Lanchat.Ipc.Commands.Blocking
{
    public class Unblock : ICommand
    {
        public string Alias => "unblock";
        public int ArgsCount => 1;

        public void Execute(string[] args)
        {
            if (!int.TryParse(args[0], out var id))
            {
                Program.IpcSocket.SendError(Error.invalid_argument);
                return;
            }

            var node = Program.NodesDatabase.GetNodeInfo(id);
            if (node == null)
            {
                Program.IpcSocket.SendError(Error.node_not_found);
                return;
            }

            node.Blocked = false;
        }
    }
}