using System.Net;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class Block
    {
        public static void Execute(string[] args)
        {
            IPAddress ipAddress;

            if (args == null || args.Length < 1)
            {
                Ui.Log.Add(Resources.Manual_Block);
                return;
            }

            if (args[0].Length == 4)
            {
                var node = Program.Network.Nodes.Find(x => x.ShortId == args[0]);
                ipAddress = node?.Endpoint.Address;
                node?.Disconnect();
            }
            else if (IPAddress.TryParse(args[0], out ipAddress))
            {
                var node = Program.Network.Nodes.Find(x => Equals(x.Endpoint.Address, ipAddress));
                node?.Disconnect();
            }
            else
            {
                Ui.Log.Add(Resources.Info_IncorrectValues);
                return;
            }

            Program.Config.AddBlocked(ipAddress);
            Ui.Log.Add($"{ipAddress} {Resources.Info_Blocked}");
        }
    }
}