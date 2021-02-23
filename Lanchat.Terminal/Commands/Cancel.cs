using System;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Cancel : ICommand
    {
        public string Alias { get; set; } = "cancel";
        public int ArgsCount { get; set; } = 1;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.ShortId == args[0]);
            if (node == null)
            {
                Ui.Log.Add(Resources._UserNotFound);
                return;
            }

            try
            {
                node.FileReceiver.CancelReceive();
            }
            catch (InvalidOperationException)
            {
                Ui.Log.Add(Resources._NoFileReceiveRequest);
            }
        }
    }
}