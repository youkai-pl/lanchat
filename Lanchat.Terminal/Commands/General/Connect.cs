using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.Commands.General
{
    public class Connect : ICommand
    {
        public string Alias => "connect";
        public int ArgsCount => 1;

        public async void Execute(string[] args)
        {
            var tabsManager = Program.Window.TabsManager;
            if (args == null || args.Length < 1)
            {
                tabsManager.WriteText(Resources.Help_connect);
                return;
            }

            var addressArgument = args[0].Trim();
            try
            {
                // If input cannot be parsed as IP try get address from dns
                if (!IPAddress.TryParse(addressArgument, out var ipAddress))
                {
                    ipAddress = (await Dns.GetHostAddressesAsync(addressArgument)).FirstOrDefault();
                }

                // Use port from argument or config
                var port = 0;
                if (args.Length > 1)
                {
                    port = int.Parse(args[1]);
                }

                if (port == 0)
                {
                    port = Program.Config.ServerPort;
                }

                tabsManager.WriteText(string.Format(Resources._ConnectionAttempt, addressArgument));
                var result = await Program.Network.Connect(ipAddress, port);
                if (!result)
                {
                    tabsManager.WriteError(string.Format(Resources._CannotConnect, ipAddress));
                }
            }
            catch (FormatException)
            {
                tabsManager.WriteError(Resources._IncorrectValues);
            }
            catch (SocketException)
            {
                tabsManager.WriteError(Resources._IncorrectValues);
            }
            catch (ArgumentException e)
            {
                tabsManager.WriteError(e.Message);
            }
        }
    }
}