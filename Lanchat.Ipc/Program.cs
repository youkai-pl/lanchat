using System;
using System.Threading;
using Lanchat.ClientCore;
using Lanchat.Core.Network;

namespace Lanchat.Ipc
{
    public static class Program
    {
        public static P2P Network { get; set; }

        public static void Main()
        {
            Console.WriteLine("Starting Lanchat IPC");
            var ipcSocket = new IpcSocket("lc.sock");
            var config = Storage.LoadConfig();
            var nodesDatabase = new NodesDatabase();

            config.DebugMode = true;
            Network = new P2P(config, nodesDatabase, x => _ = new NodeHandlers(x.Instance, ipcSocket));

            Network.Start();
            ipcSocket.Start();
        }
    }
}