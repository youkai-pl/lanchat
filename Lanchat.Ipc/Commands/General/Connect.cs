using System;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Ipc.Commands.General
{
    public class Connect : ICommand
    {
        public string Alias => "connect";
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
                    Program.IpcSocket.Send("cannot_connect");
                }
            }
            catch (FormatException)
            {
                Program.IpcSocket.Send("invalid_argument");
            }
            catch (SocketException)
            {
                Program.IpcSocket.Send("invalid_argument");
            }
            catch (ArgumentException)
            {
                Program.IpcSocket.Send("invalid_argument");
            }
        }
    }
}