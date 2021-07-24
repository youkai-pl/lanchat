using System.Linq;
using System.Net;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.Commands.Blocking
{
    public class Unblock : ICommand
    {
        public string Alias => "unblock";
        public int ArgsCount => 1;

        public void Execute(string[] args)
        {
            var tabsManager = Program.Window.TabsManager;
            var correct = IPAddress.TryParse(args[0], out var parsedIp);
            if (!correct)
            {
                tabsManager.WriteError(Resources._IncorrectValues);
                return;
            }

            if (Program.Config.BlockedAddresses.All(x => !Equals(x, parsedIp)))
            {
                tabsManager.WriteError(Resources._UserNotFound);
                return;
            }

            Program.Config.BlockedAddresses.Remove(parsedIp);
            tabsManager.WriteError(string.Format(Resources._Unblocked, parsedIp));
        }
    }
}