using Lanchat.Core;
using Lanchat.Terminal.Ui;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lanchat.Terminal.Handlers;

namespace Lanchat.Terminal
{
    public static class Program
    {
        public static Config Config { get; set; }
        public static P2P Network { get; private set; }

        private static void Main(string[] args)
        {
            Config = Config.Load();
            Core.Config.Nickname = Config.Nickname;
            Network = new P2P(3645);
            Network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node); };
            Prompt.Start(Config, Network);
            Network.Start();
            
            if (Array.IndexOf(args, "-debug") > -1 || Debugger.IsAttached)
            {
                Trace.WriteLine(Properties.Resources._DebugMode);
                Trace.Listeners.Add(new TerminalTraceListener());
            }

            Trace.Listeners.Add(new FileTraceListener($"{Config.Path}{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log"));
            Trace.IndentSize = 11;
            Trace.AutoFlush = true;
            Trace.WriteLine("[APP] Logging started");

            foreach (var fi in new DirectoryInfo(Config.Path).GetFiles("*.log").OrderByDescending(x => x.LastWriteTime).Skip(5))
            {
                fi.Delete();
            }
        }
    }
}