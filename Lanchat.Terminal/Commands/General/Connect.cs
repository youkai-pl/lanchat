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

                bool connected;
                if (IPAddress.TryParse(addressArgument, out var ipAddress))
                {
                    Writer.WriteText(string.Format(Resources.ConnectionAttempt, addressArgument));
                    connected = await Program.Network.Connect(ipAddress, port);
                }
                else
                {
                    Writer.WriteText(string.Format(Resources.ConnectionAttemptDns, addressArgument));
                    connected = await Program.Network.Connect(addressArgument, port);
                }

                if (!connected)
                {
                    Writer.WriteError(Resources.CannotConnectCommand);
                }
            }
            catch (FormatException)
            {
                Writer.WriteError(Resources.IncorrectCommandUsage);
            }
            catch (ArgumentException)
            {
                Writer.WriteError(Resources.IncorrectCommandUsage);
            }
            catch (SocketException)
            {
                Writer.WriteError(Resources.CannotConnectCommand);
            }
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}