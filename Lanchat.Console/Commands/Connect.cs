using System;
using System.Net;
using System.Threading;
using Lanchat.Console.Ui;
using Lanchat.Common.NetworkLib;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void Connect(string ip, string port)
        {
            new Thread(() =>
            {
                try
                {
                    var parsedIP = IPAddress.Parse(ip);
                    var parsedPort = int.Parse(port);
                    Prompt.Notice($"Attempting connect to {ip} on port {port}");
                    try
                    {

                        program.Network.Methods.Connect(parsedIP, parsedPort);

                    }
                    catch (ConnectionFailedException)
                    {
                        Prompt.Alert("Manual connection failed");
                    }
                    catch (NodeAlreadyExistException)
                    {
                        Prompt.Alert("Node already connected");
                    }
                }
                catch (Exception e)
                {
                    if (e is ArgumentNullException || e is FormatException)
                    {
                        Prompt.Alert("Incorrect values");
                        return;
                    }
                }
            }).Start();
        }
    }
}