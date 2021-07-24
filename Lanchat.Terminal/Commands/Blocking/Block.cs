using System.Linq;
using System.Net;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.Commands.Blocking
{
    public class Block : ICommand
    {
        public string Alias => "block";
        public int ArgsCount => 1;

        public void Execute(string[] args)
        {
            var tabsManager = Program.Window.TabsManager;
            IPAddress ipAddress;

            if (args == null || args.Length < 1)
            {
                tabsManager.WriteError(Resources.Help_block);
                return;
            }

            if (args[0].Length == 4)
            {
                var node = Program.Network.Nodes.Find(x => x.User.ShortId == args[0]);
                ipAddress = node?.Host.Endpoint.Address;
                node?.Disconnect();
            }
            else if (IPAddress.TryParse(args[0], out ipAddress))
            {
                var node = Program.Network.Nodes.Find(x => Equals(x.Host.Endpoint.Address, ipAddress));
                node?.Disconnect();
            }
            else
            {
                tabsManager.WriteError(Resources._IncorrectValues);
                return;
            }

            if (Program.Config.BlockedAddresses.Any(x => Equals(x, ipAddress)))
            {
                tabsManager.WriteError(Resources._AlreadyBlocked);
                return;
            }

            Program.Config.BlockedAddresses.Add(ipAddress);
            tabsManager.WriteText(string.Format(Resources._Blocked, ipAddress));
        }
    }
}