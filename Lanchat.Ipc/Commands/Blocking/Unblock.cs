using System.Net;

namespace Lanchat.Ipc.Commands.Blocking
{
    public class Unblock : ICommand
    {
        public string Alias => "unblock";
        public int ArgsCount => 1;

        public void Execute(string[] args)
        {
            var correct = IPAddress.TryParse(args[0], out var ipAddress);
            if (!correct)
            {
                return;
            }

            var node = Program.NodesDatabase.GetNodeInfo(ipAddress);
            if (node == null)
            {
                Program.IpcSocket.SendError(Error.node_not_found);
                return;
            }

            node.Blocked = false;
        }
    }
}