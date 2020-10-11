using Lanchat.Terminal.Ui;
using System;
using System.Net;
using Lanchat.Core;

namespace Lanchat.Terminal.Commands
{
    public static class Connect
    {
        public static void Execute(string[] args, P2P network)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Length < 2)
            {
                Prompt.Log.Add(Properties.Resources.Manual_Connect);
                return;
            }


            try
            {
                var parsedIp = IPAddress.Parse(args[0]);
                Prompt.Log.Add(
                    $"{Properties.Resources._ConnectionAttempt} {args[0]} {Properties.Resources._OnPort} {args[1]}");
                network.Connect(parsedIp);
            }
            catch (Exception e)
            {
                if (!(e is ArgumentNullException) && !(e is FormatException))
                {
                    throw;
                }

                Prompt.Log.Add(Properties.Resources._IncorrectValues);
            }
        }
    }
}