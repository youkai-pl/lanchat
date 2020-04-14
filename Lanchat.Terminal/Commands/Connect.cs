using Lanchat.Common.NetworkLib;
using Lanchat.Common.NetworkLib.Exceptions;
using Lanchat.Terminal.Ui;
using System;
using System.Globalization;
using System.Net;
using System.Threading;

namespace Lanchat.Terminal.Commands
{
    public static class Connect
    {
        public static void Execute(string[] args, Network network)
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

            new Thread(() =>
            {
                try
                {
                    var parsedIP = IPAddress.Parse(args[0]);
                    var parsedPort = int.Parse(args[1], CultureInfo.CurrentCulture);
                    Prompt.Log.Add($"{Properties.Resources._ConnectionAttempt} {args[0]} {Properties.Resources._OnPort} {args[1]}");
                    try
                    {
                        network.Methods.Connect(parsedIP, parsedPort);
                    }
                    catch (Exception e)
                    {
                        if (e is ConnectionFailedException)
                        {
                            Prompt.Log.Add(Properties.Resources._ManualConnectionFailed);
                        }
                        else if (e is NodeAlreadyExistException)
                        {
                            Prompt.Log.Add(Properties.Resources._AlreadyConnected);
                        }
                        throw;
                    }
                }
                catch (Exception e)
                {
                    if (e is ArgumentNullException || e is FormatException)
                    {
                        Prompt.Log.Add(Properties.Resources._IncorrectValues);
                        return;
                    }
                    throw;
                }
            }).Start();
        }
    }
}
