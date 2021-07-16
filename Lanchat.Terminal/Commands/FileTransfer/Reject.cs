using System;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.Commands.FileTransfer
{
    public class Reject : ICommand
    {
        public string Alias => "reject";
        public int ArgsCount => 1;

        public void Execute(string[] args)
        {
            var tabsManager = Program.Window.TabsManager;
            var node = Program.Network.Nodes.Find(x => x.User.ShortId == args[0]);
            if (node == null)
            {
                tabsManager.WriteError(Resources._UserNotFound);
                return;
            }

            try
            {
                node.FileReceiver.RejectRequest();
            }
            catch (InvalidOperationException)
            {
                tabsManager.WriteError(Resources._NoFileReceiveRequest);
            }
        }
    }
}