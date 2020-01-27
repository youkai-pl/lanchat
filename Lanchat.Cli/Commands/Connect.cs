using System.Net;
using System.Threading;
using Lanchat.Cli.Ui;

namespace Lanchat.Cli.Commands
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
                    catch
                    {
                        Prompt.Alert("Manual connection failed");
                    };
                }
                catch
                {
                    Prompt.Alert("Incorrect values");
                }
            }).Start();
        }
    }
}