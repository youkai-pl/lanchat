using System;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.FileTransfer
{
    public class Accept : ICommand
    {
        public string[] Aliases { get; } =
        {
            "accept"
        };
        public int ArgsCount => 1;
        public int ContextArgsCount => 0;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.User.ShortId == args[0]);
            if (node == null)
            {
                Writer.WriteError(Resources.UserNotFound);
                return;
            }

            Execute(args, node);
        }

        public void Execute(string[] args, INode node)
        {
            try
            {
                node.FileReceiver.AcceptRequest();
                Writer.WriteStatus(Resources.FileTransferAcceptCommand);
            }
            catch (InvalidOperationException)
            {
                Writer.WriteError(Resources.NoFileReceiveRequest);
            }
        }
    }
}