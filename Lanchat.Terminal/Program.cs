using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using Lanchat.Core;
using Lanchat.Terminal.Handlers;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.Ui;

namespace Lanchat.Terminal
{
    public static class Program
    {
        private static Config _config;
        public static P2P Network { get; private set; }

        private static void Main(string[] args)
        {
            _config = Config.Load();
            CoreConfig.Nickname = _config.Nickname;
            CoreConfig.ServerPort = _config.Port;
            CoreConfig.BlockedAddresses = _config.BlockedAddresses.Select(IPAddress.Parse).ToList();
            
            Network = new P2P();
            Network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node); };
            Network.Start();

            Prompt.Start(_config, Network);

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