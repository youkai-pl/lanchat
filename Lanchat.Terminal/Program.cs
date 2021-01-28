using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.ClientCore;
using Lanchat.Core;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal
{
    public static class Program
    {
        public static P2P Network { get; private set; }
        public static Config Config { get; private set; }

        private static void Main(string[] args)
        {
            Config = Config.Load();

            // Initialize server mode
            if (args.Contains("--server") || args.Contains("-s"))
            {
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
                LoggingService.StartLogging();

                if (Config.UseIPv6)
                {
                    var server = new Server(IPAddress.IPv6Any, CoreConfig.ServerPort);
                    server.Start();
                }
                else
                {
                    var server = new Server(IPAddress.Any, CoreConfig.ServerPort);
                    server.Start();
                }

                LoggingService.CleanLogs();
                while (true) Console.ReadKey();
            }

            // Load resources
            Resources.Culture = new CultureInfo(Config.Language);

            // Initialize p2p mode and ui
            try
            {
                Ui.Start();
                Network = new P2P();
                Network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node); };

                // Initialize server
                if (!args.Contains("--no-server") && !args.Contains("-n")) Network.StartServer();

                // Start broadcast service
                if (!args.Contains("--no-udp") && !args.Contains("-b")) Network.StartBroadcast();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.AddressAlreadyInUse)
                    Ui.Log.Add(Resources.Info_PortBusy);
                else
                    throw;
            }

            // Show logs in console
            if (args.Contains("--debug") || args.Contains("-d") || Debugger.IsAttached)
                Trace.Listeners.Add(new TerminalTraceListener());

            // Save logs to file
            LoggingService.StartLogging();

            // Connect with localhost
            if (args.Contains("--loopback") || args.Contains("-l")) Network.Connect(IPAddress.Loopback);

            var newVersion = UpdateChecker.CheckUpdates();
            if (newVersion != null) Ui.StatusBar.Text = Ui.StatusBar.Text += $" - Update available ({newVersion})";

            LoggingService.CleanLogs();
        }
    }
}