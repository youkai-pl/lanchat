using System.Net;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.Blocking
{
    public class Unblock : ICommand
    {
        public string Alias => "unblock";
        public int ArgsCount => 1;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] args)
        {
            var correct = IPAddress.TryParse(args[0], out var ipAddress);
            if (!correct)
            {
                Writer.WriteError(Resources._IncorrectValues);
                return;
            }

            var node = Program.NodesDatabase.GetNodeInfo(ipAddress);
            if (node == null)
            {
                Writer.WriteError(Resources._AlreadyBlocked);
                return;
            }

            node.Blocked = false;
            Writer.WriteText(string.Format(Resources._Unblocked, ipAddress));
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}