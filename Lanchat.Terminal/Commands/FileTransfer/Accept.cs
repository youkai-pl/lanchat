using System;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.Commands.FileTransfer
{
    public class Accept : ICommand
    {
        public string Alias => "accept";
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
                node.FileReceiver.AcceptRequest();
            }
            catch (InvalidOperationException)
            {
                tabsManager.WriteError(Resources._NoFileReceiveRequest);
            }
        }
    }
}