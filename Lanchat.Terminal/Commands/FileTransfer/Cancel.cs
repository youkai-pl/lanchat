using System;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.FileTransfer
{
    public class Cancel : ICommand
    {
        public string Alias => "cancel";
        public int ArgsCount => 1;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.User.ShortId == args[0]);
            if (node == null)
            {
                Window.Writer.WriteError(Resources._UserNotFound);
                return;
            }

            try
            {
                node.FileReceiver.CancelReceive(true);
            }
            catch (InvalidOperationException)
            {
                Window.Writer.WriteError(Resources._NoFileReceiveRequest);
            }
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}