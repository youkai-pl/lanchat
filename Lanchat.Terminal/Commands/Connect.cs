using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;
using static System.Int32;

namespace Lanchat.Terminal.Commands
{
    public static class Connect
    {
        public static void Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Ui.Log.Add(Resources.Manual_Connect);
                return;
            }

            var addressArgument = args[0].Trim();
            try
            {
                // If input cannot be parsed as IP try get address from dns
                if (!IPAddress.TryParse(addressArgument, out var ipAddress))
                {
                    ipAddress = Dns.GetHostAddresses(addressArgument).FirstOrDefault();
                }
                
                // Use port from argument or config
                var port = 0;
                if (args.Length > 1)
                {
                    port = Parse(args[1]);
                }
                
                if (port == 0)
                {
                    port = CoreConfig.ServerPort;
                }
                
                Ui.Log.Add($"{Resources.Info_ConnectionAttempt} {addressArgument}");
                Program.Network.Connect(ipAddress, port);
            }
            catch (FormatException)
            {
                Ui.Log.Add(Resources.Info_IncorrectValues);
            }
            catch (SocketException)
            {
                Ui.Log.Add(Resources.Info_IncorrectValues);
            }
            catch (ArgumentException e)
            {
                Ui.Log.Add(e.Message);
            }
        }
    }
}