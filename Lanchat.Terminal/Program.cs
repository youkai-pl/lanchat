using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lanchat.Core;
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
            Ui.Start();

            // Enable debug mode
            if (Array.IndexOf(args, "-debug") > -1 || Debugger.IsAttached)
            {
                Trace.Listeners.Add(new TerminalTraceListener());
            }

            // Save logs to file
            Trace.Listeners.Add(new FileTraceListener($"{Config.Path}{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log"));
            Trace.IndentSize = 11;
            Trace.AutoFlush = true;
            Trace.WriteLine("[APP] Logging started");
            
            // Initialize network
            Network = new P2P();
            Network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node); };
            Network.Start();

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