using System.Net;
using Lanchat.Core;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class Block
    {
        public static void Execute(string[] args, Config config, P2P network)
        {
            if (args == null || args.Length < 1)
            {
                Ui.Log.Add(Resources.Manual_Block);
                return;
            }

            var correct = IPAddress.TryParse(args[0], out var parsedIp);
            if (correct)
            {
                config.AddBlocked(parsedIp);
                network.Nodes.Find(x => Equals(x.Endpoint.Address, parsedIp))?.Disconnect();
                Ui.Log.Add($"{parsedIp} blocked");
            }
            else
            {
                Ui.Log.Add(Resources._IncorrectValues);
            }
        }
    }
}