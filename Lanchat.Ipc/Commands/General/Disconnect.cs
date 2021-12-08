using System;

namespace Lanchat.Ipc.Commands.General
{
    public class Disconnect : ICommand
    {
        public string Alias => "disconnect";
        public int ArgsCount => 1;

        public void Execute(string[] args)
        {
            var guid = Guid.Parse(args[0]);
            var node = Program.Network.Nodes.Find(x => x.Id == guid);
            if (node != null)
            {
                node.Disconnect();
            }
            else
            {
                Program.IpcSocket.Send("node_not_found");
            }
        }
    }
}