using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

            // Initialize server
            if (args.Contains("--server") || args.Contains("-s"))
            {
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
                var server = new Server(IPAddress.Any, CoreConfig.ServerPort);
                server.Start();
                CleanLogs();
                while (true)
                {
                    Console.ReadKey();
                }
            }

            // Initialize p2p mode and ui
            try
            {
                Ui.Start();
                Network = new P2P();
                Network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node); };
                Network.Start();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.AddressNotAvailable)
                {
                    Ui.Log.Add(Resources.Info_PortBusy);
                }
                else
                {
                    throw;
                }
            }

            // Enable logging
            if (args.Contains("--debug") || args.Contains("-d") || Debugger.IsAttached)
            {
                Trace.Listeners.Add(new TerminalTraceListener());
            }

            // Save logs to file
            Trace.Listeners.Add(new FileTraceListener($"{Config.Path}{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log"));
            Trace.IndentSize = 11;
            Trace.AutoFlush = true;
            Trace.WriteLine("Logging started");


            // Connect with localhost
            if (args.Contains("--loopback") || args.Contains("-l"))
            {
                Network.Connect(IPAddress.Loopback);
            }

            CleanLogs();
        }

        private static void CleanLogs()
        {
            foreach (var fi in new DirectoryInfo(Config.Path)
                .GetFiles("*.log")
                .OrderByDescending(x => x.LastWriteTime)
                .Skip(5))
            {
                fi.Delete();
            }
        }
    }
}