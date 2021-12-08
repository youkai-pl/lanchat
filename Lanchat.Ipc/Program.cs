using System;
using Lanchat.ClientCore;
using Lanchat.Core.Network;
using Lanchat.Ipc.Commands;

namespace Lanchat.Ipc
{
    public static class Program
    {
        public static Config Config {get; set;}
        public static CommandsManager CommandsManager {get;set;}
        public static P2P Network { get; set; }
        public static IpcSocket IpcSocket { get; set; }

        public static void Main()
        {
            Console.WriteLine("Starting Lanchat IPC");
            Config = Storage.LoadConfig();
            Config.DebugMode = true;

            CommandsManager = new CommandsManager();
            IpcSocket = new IpcSocket("lc.sock");
            Network = new P2P(Config, new NodesDatabase(), x => _ = new NodeHandlers(x.Instance));

            Network.Start();
            IpcSocket.Start();
        }
    }
}