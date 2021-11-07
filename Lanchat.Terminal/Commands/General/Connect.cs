using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.General
{
    public class Connect : ICommand
    {
        public string Alias => "connect";
        public int ArgsCount => 1;
        public int ContextArgsCount => ArgsCount;

        public async void Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Writer.WriteText(Resources.Help_connect);
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

                Writer.WriteText(string.Format(Resources.ConnectionAttempt, addressArgument));
                var result = await Program.Network.Connect(ipAddress, port);
                if (!result)
                {
                    Writer.WriteError(string.Format(Resources.CannotConnect, ipAddress));
                }
            }
            catch (FormatException)
            {
                Writer.WriteError(Resources.IncorrectValues);
            }
            catch (SocketException)
            {
                Writer.WriteError(Resources.IncorrectValues);
            }
            catch (ArgumentException e)
            {
                Writer.WriteError(e.Message);
            }
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}