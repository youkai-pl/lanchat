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
        public string[] Aliases { get; } =
        {
            "connect",
            "c"
        };
        public int ArgsCount => 1;
        public int ContextArgsCount => ArgsCount;

        public async void Execute(string[] args)
        {
            var addressArgument = args[0].Trim();
            try
            {
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

                bool result;
                if (IPAddress.TryParse(addressArgument, out var ipAddress))
                {
                    result = await Program.Network.Connect(ipAddress, port);
                }
                else
                {
                    result = await Program.Network.Connect(addressArgument, port);
                }

                if (!result)
                {
                    Writer.WriteError(string.Format(Resources.CannotConnect, ipAddress));
                }
            }
            catch (FormatException)
            {
                Writer.WriteError(Resources.IncorrectCommandUsage);
            }
            catch (SocketException)
            {
                Writer.WriteError(Resources.IncorrectCommandUsage);
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