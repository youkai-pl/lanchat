using System;
using System.Net;
using Lanchat.Core;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.Ui;

namespace Lanchat.Terminal.Commands
{
    public static class Connect
    {
        public static void Execute(string[] args, P2P network)
        {
            if (args == null || args.Length < 2)
            {
                Prompt.Log.Add(Resources.Manual_Connect);
                return;
            }

            try
            {
                var parsedIp = IPAddress.Parse(args[0]);
                Prompt.Log.Add($"{Resources._ConnectionAttempt} {args[0]}");
                network.Connect(parsedIp);
            }
            catch (Exception e)
            {
                if (!(e is ArgumentNullException) && !(e is FormatException))
                {
                    throw;
                }

                Prompt.Log.Add(Resources._IncorrectValues);
            }
        }
    }
}