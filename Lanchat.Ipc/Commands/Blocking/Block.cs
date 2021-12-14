using System.Net;
using Lanchat.Core.Network;

namespace Lanchat.Ipc.Commands.Blocking
{
    public class Block : ICommand
    {
        public string Alias => "block";

        public int ArgsCount => 2;

        public void Execute(string[] args)
        {
            switch (args[0])
            {
                case "id":

                    BlockById(args[1]);
                    break;

                case "ip":
                    BlockByAddress(args[1]);
                    break;
            }
        }

        private static void BlockById(string id)
        {
            var node = Program.Network.Nodes.Find(x => x.User.ShortId == id);
            if (node == null)
            {
                Program.IpcSocket.SendError(Error.node_not_found);
                return;
            }

            BlockConnectedNode(node);
        }

        private static void BlockByAddress(string address)
        {
            if (IPAddress.TryParse(address, out var ipAddress))
            {
                var node = Program.Network.Nodes.Find(x => Equals(x.Host.Endpoint.Address, ipAddress));
                if (node == null)
                {
                    SaveBlockInConfig(ipAddress);
                }
                else
                {
                    BlockConnectedNode(node);
                }
            }
            else
            {
                Program.IpcSocket.SendError(Error.invalid_argument);
            }
        }

        private static void BlockConnectedNode(INode node)
        {
            var ipAddress = node.Host.Endpoint.Address;
            node.Disconnect();
            SaveBlockInConfig(ipAddress);
        }

        private static void SaveBlockInConfig(IPAddress ipAddress)
        {
            var node = Program.NodesDatabase.GetNodeInfo(ipAddress);
            node.Blocked = true;
        }
    }
}