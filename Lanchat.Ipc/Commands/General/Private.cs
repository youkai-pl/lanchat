using System;
using System.Linq;

namespace Lanchat.Ipc.Commands.General
{
    public class Private : ICommand
    {
        public string Alias => "private";
        public int ArgsCount => 2;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.User.ShortId == args[0]);
            if (node != null)
            {
                node.Messaging.SendPrivateMessage(string.Join(" ", args.Skip(1)));
            }
            else
            {
                Program.IpcSocket.SendError(Error.node_not_found);
            }
        }
    }
}