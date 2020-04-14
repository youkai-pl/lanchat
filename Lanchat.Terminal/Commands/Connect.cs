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
            new Thread(() =>
            {
                try
                {
                    var parsedIP = IPAddress.Parse(args[0]);
                    var parsedPort = int.Parse(args[1], CultureInfo.CurrentCulture);
                    Prompt.Log.Add($"{Properties.Resources._ConnectionAttempt} {args[0]} {Properties.Resources._OnPort} {args[1]}", Prompt.OutputType.System);
                    try
                    {
                        network.Methods.Connect(parsedIP, parsedPort);
                    }
                    catch (Exception e)
                    {
                        if (e is ConnectionFailedException)
                        {
                            Prompt.Log.Add(Properties.Resources._ManualConnectionFailed, Prompt.OutputType.System);
                        }
                        else if (e is NodeAlreadyExistException)
                        {
                            Prompt.Log.Add(Properties.Resources._AlreadyConnected, Prompt.OutputType.System);
                        }
                        throw;
                    }
                }
                catch (Exception e)
                {
                    if (e is ArgumentNullException || e is FormatException)
                    {
                        Prompt.Log.Add(Properties.Resources._IncorrectValues, Prompt.OutputType.System);
                        return;
                    }
                    throw;
                }
            }).Start();
        }
    }
}
