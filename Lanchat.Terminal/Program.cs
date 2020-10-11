using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lanchat.Core;
using Lanchat.Terminal.Handlers;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.Ui;

namespace Lanchat.Terminal
{
    public static class Program
    {
        private static Config _config;
        private static P2P _network;

        private static void Main(string[] args)
        {
            _config = Config.Load();
            Core.Config.Nickname = _config.Nickname;
            _network = new P2P(3645);
            _network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node); };
            Prompt.Start(_config, _network);
            _network.Start();

            if (Array.IndexOf(args, "-debug") > -1 || Debugger.IsAttached)
            {
                Trace.WriteLine(Resources._DebugMode);
                Trace.Listeners.Add(new TerminalTraceListener());
            }

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