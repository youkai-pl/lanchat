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
                Trace.WriteLine(ip);
                Trace.WriteLine(IPAddress.Parse(ip));
                Trace.WriteLine("test");
                //program.Network.Out.Connect(IPAddress.Parse(ip), int.Parse(port));
            }
            catch(Exception e)
            {
                Prompt.Alert("Cannot create connection");
                Trace.WriteLine(e.Message);
            }
        }
    }
}