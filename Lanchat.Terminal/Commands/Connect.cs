using System;
using System.Net;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

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

            try
            {
                var parsedIp = IPAddress.Parse(args[0]);
                Ui.Log.Add($"{Resources.Info_ConnectionAttempt} {args[0]}");
                Program.Network.Connect(parsedIp);
            }
            catch (FormatException)
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