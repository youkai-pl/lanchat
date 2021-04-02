using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.ClientCore;
using Lanchat.Core;
using Lanchat.Core.Network;
using Lanchat.Core.P2P;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal
{
    public static class Program
    {
        public static Network Network { get; private set; }
        public static Config Config { get; private set; }

        private static void Main(string[] args)
        {
            Config = ConfigManager.Load();

            // Load resources
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(Config.Language);
            }
            catch
            {
                Trace.WriteLine("Cannot load translation. Using default.");
            }

            Resources.Culture = CultureInfo.CurrentCulture;

            // Initialize p2p mode and ui
            try
            {
                Ui.Start();
                Network = new Network(Config);
                Network.NodeCreated += (sender, node) => { _ = new NodeEventsHandlers(node); };

                // Initialize server
                if (!args.Contains("--no-server") && !args.Contains("-n")) Network.StartServer();

                // Start broadcast service
                if (!args.Contains("--no-udp") && !args.Contains("-b")) Network.StartNodesDetection();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.AddressAlreadyInUse)
                    Ui.Log.AddWarning(Resources._PortBusy);
                else
                    throw;
            }

            Ui.SetupNetworkEvents();

            // Show logs in console
            if (args.Contains("--debug") || args.Contains("-d") || Debugger.IsAttached)
            {
                Trace.Listeners.Add(new TerminalTraceListener());
            }
            else
            {
                var newVersion = UpdateChecker.CheckUpdates();
                if (newVersion != null) Ui.StatusBar.Text = Ui.StatusBar.Text += $" - Update available ({newVersion})";
            }

            // Save logs to file
            LoggingService.StartLogging();

            // Connect with localhost
            if (args.Contains("--loopback") || args.Contains("-l")) Network.Connect(IPAddress.Loopback);

            if (!args.Contains("--no-auto") && !args.Contains("-a")) Network.AutoConnect();

            LoggingService.CleanLogs();
        }
    }
}