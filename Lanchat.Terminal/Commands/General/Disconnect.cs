using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.Commands.General
{
    public class Disconnect : ICommand
    {
        public string Alias => "disconnect";
        public int ArgsCount => 1;

        public void Execute(string[] args)
        {
            var tabsManager = Program.Window.TabsManager;
            var node = Program.Network.Nodes.Find(x => x.User.ShortId == args[0]);
            if (node != null)
            {
                node.Disconnect();
            }
            else
            {
                tabsManager.WriteError(Resources._UserNotFound);
            }
        }
    }
}