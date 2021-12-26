using System;
using System.Diagnostics;
using System.Linq;
using Lanchat.ClientCore;
using Lanchat.Core.Network;
using Lanchat.Ipc.Commands;

namespace Lanchat.Ipc
{
    public static class Program
    {
        public static Config Config { get; set; }
        public static NodesDatabase NodesDatabase { get; set; }
        public static CommandsManager CommandsManager { get; set; }
        public static P2P Network { get; set; }
        public static IpcSocket IpcSocket { get; set; }

        public static void Main(string[] args)
        {
            Config = ConfigLoader.LoadConfig();
            CheckStartArguments(args);

            Config.DebugMode = true;

            NodesDatabase = new NodesDatabase();
            CommandsManager = new CommandsManager();
            IpcSocket = new IpcSocket(args[0]);
            Network = new P2P(Config, NodesDatabase, x => _ = new NodeHandlers(x.Instance));

            Network.Start();
            IpcSocket.Start();
        }

        private static void CheckStartArguments(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Invalid usage");
                Environment.Exit(1);
            }

            if (args.Contains("-d") || Debugger.IsAttached)
            {
                Config.DebugMode = true;
            }

            if (args.Contains("-a"))
            {
                Config.ConnectToSaved = false;
            }

            if (args.Contains("-n"))
            {
                Config.StartServer = false;
            }

            if (args.Contains("-b"))
            {
                Config.NodesDetection = false;
            }
        }
    }
}