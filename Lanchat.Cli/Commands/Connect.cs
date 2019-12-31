using System;
using System.Diagnostics;
using System.Net;
using Lanchat.Cli.Ui;

namespace Lanchat.Cli.Commands
{
    public partial class Command
    {
        public void Connect(string ip, string port)
        {
            try
            {

                var parsedIP = IPAddress.Parse(ip);
                var parsedPort = int.Parse(port);
                Prompt.Notice($"Attempting connect to {ip} on port {port}");
                try
                {
                    program.Network.Connect(parsedIP, parsedPort);
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Connect command error: " + e.GetType());
                    Prompt.Alert("Manual connection failed");
                };
            }
            catch (Exception e)
            {
                Trace.WriteLine("Connect command error: " + e.GetType());
                Prompt.Alert("Incorrect values");
            }
        }
    }
}