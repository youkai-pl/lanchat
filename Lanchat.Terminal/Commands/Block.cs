using System.Net;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Block : ICommand
    {
        public string Alias { get; } = "block";
        public int ArgsCount { get; } = 1;

        public void Execute(string[] args)
        {
            IPAddress ipAddress;

            if (args == null || args.Length < 1)
            {
                Ui.Log.Add(Resources.Help_block);
                return;
            }

            if (args[0].Length == 4)
            {
                var node = Program.Network.Nodes.Find(x => x.ShortId == args[0]);
                ipAddress = node?.NetworkElement.Endpoint.Address;
                node?.Disconnect();
            }
            else if (IPAddress.TryParse(args[0], out ipAddress))
            {
                var node = Program.Network.Nodes.Find(x => Equals(x.NetworkElement.Endpoint.Address, ipAddress));
                node?.Disconnect();
            }
            else
            {
                Ui.Log.Add(Resources._IncorrectValues);
                return;
            }

            Program.Config.AddBlocked(ipAddress);
            Ui.Log.Add(string.Format(Resources._Blocked, ipAddress));
        }
    }
}