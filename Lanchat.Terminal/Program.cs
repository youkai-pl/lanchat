using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
            // Load config
            Config = Config.Load();

            // Initialize network and start UI
            Network = new P2P();
            Network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node); };
            Network.Start();
            Ui.Start();

            // Enable debug mode
            if (Array.IndexOf(args, "-debug") > -1 || Debugger.IsAttached)
            {
                Trace.Listeners.Add(new TerminalTraceListener());
                Trace.WriteLine(Resources._DebugMode);
            }

            // Save logs to file
            Trace.Listeners.Add(new FileTraceListener($"{Config.Path}{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log"));
            Trace.IndentSize = 11;
            Trace.AutoFlush = true;
            Trace.WriteLine("[APP] Logging started");

            // Remove old logs
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