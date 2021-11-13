using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.ClientCore;
using Lanchat.Core.Network;
using Lanchat.Terminal.Commands;
using Lanchat.Terminal.Handlers;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal
{
    public static class Program
    {
        public static IP2P Network { get; private set; }
        public static Config Config { get; private set; }
        public static NodesDatabase NodesDatabase { get; private set; }
        public static CommandsManager CommandsManager {get; private set;}

        private static void Main(string[] args)
        {
            Config = Storage.LoadConfig();
            NodesDatabase = new NodesDatabase();
            CommandsManager = new CommandsManager();

            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(Config.Language);
            }
            catch
            {
                Trace.WriteLine("Cannot load translation. Using default.");
            }

            Resources.Culture = CultureInfo.CurrentCulture;

            Network = new P2P(Config, NodesDatabase, x =>
            {
                _ = new NodeHandlers(x.Instance);
                _ = new FileTransferHandlers(x.Instance);
            });

            CheckStartArguments(args);
            Window.Initialize();
            Logger.StartLogging();

            try
            {
                Network.Start();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.AddressAlreadyInUse)
                {
                    throw;
                }

                TabsManager.HomeView.AddText(Resources.PortBusy, ConsoleColor.Yellow);
            }

            if (args.Contains("--localhost") || args.Contains("-l"))
            {
                Network.Connect(IPAddress.Loopback);
            }

            Logger.DeleteOldLogs(5);
        }

        private static void CheckStartArguments(string[] args)
        {
            if (args.Contains("--no-saved") || args.Contains("-a"))
            {
                Config.ConnectToSaved = false;
            }

            if (args.Contains("--no-udp") || args.Contains("-b"))
            {
                Config.NodesDetection = false;
            }

            if (args.Contains("--no-server") || args.Contains("-n"))
            {
                Config.StartServer = false;
            }

            if (args.Contains("--debug") || args.Contains("-d") || Debugger.IsAttached)
            {
                Config.DebugMode = true;
                Trace.Listeners.Add(new TraceListener());
            }
            else
            {
                var newVersion = UpdateChecker.CheckUpdates();
                if (newVersion != null)
                {
                    TabsManager.HomeView.AddText($"Update available: {newVersion}", ConsoleColor.Green);
                }
            }
        }
    }
}