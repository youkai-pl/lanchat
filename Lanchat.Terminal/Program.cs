using System;
using System.Diagnostics;
using Lanchat.Terminal.Ui;
using Lanchat.Common.NetworkLib;

namespace Lanchat.Terminal
{
    class Program
    {
        internal static Config Config { get; set; }
        internal static Network Network { get; set; }
        internal static NetworkEventsHandlers NetworkEventsHandlers { get; set; }

        static void Main(string[] args)
        {
            if (Array.IndexOf(args, "-debug") > -1 || Debugger.IsAttached)
            {
                Trace.WriteLine(Properties.Resources.Alert_DebugMode);
            }

            Config = Config.Load();

            NetworkEventsHandlers = new NetworkEventsHandlers(Config, Network);
            Network = new Network(Config.BroadcastPort, Config.Nickname, Config.HostPort, Config.HeartbeatTimeout, Config.ConnectionTimeout);
            Network.Events.HostStarted += NetworkEventsHandlers.OnHostStarted;
            Network.Events.ReceivedMessage += NetworkEventsHandlers.OnReceivedMessage;
            Network.Events.NodeConnected += NetworkEventsHandlers.OnNodeConnected;
            Network.Events.NodeDisconnected += NetworkEventsHandlers.OnNodeDisconnected;
            Network.Events.NodeSuspended += NetworkEventsHandlers.OnNodeSuspended;
            Network.Events.NodeResumed += NetworkEventsHandlers.OnNodeResumed;
            Network.Events.ChangedNickname += NetworkEventsHandlers.OnChangedNickname;

            Prompt.Start(Config, Network);
            Network.Start();
        }
    }
}
