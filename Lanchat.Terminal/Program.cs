using System;
using System.Diagnostics;
using Lanchat.Terminal.Ui;
using Lanchat.Common.NetworkLib;
using System.IO;
using System.Linq;

namespace Lanchat.Terminal
{
    class Program
    {
        internal static Config Config { get; set; }
        internal static Network Network { get; set; }
        internal static NetworkEventsHandlers NetworkEventsHandlers { get; set; }

        static void Main(string[] args)
        {

            Config = Config.Load();
            Prompt.Start(Config);

            try
            {
                Network = new Network(
                    Config.BroadcastPort,
                    Config.Nickname,
                    Config.HostPort,
                    Config.HeartbeatSendTimeout,
                    Config.ConnectionTimeout
                );
                Prompt.InputController.SetNetwork(Network);
                NetworkEventsHandlers = new NetworkEventsHandlers(Config, Network);
                Network.Events.HostStarted += NetworkEventsHandlers.OnHostStarted;
                Network.Events.ReceivedMessage += NetworkEventsHandlers.OnReceivedMessage;
                Network.Events.NodeConnected += NetworkEventsHandlers.OnNodeConnected;
                Network.Events.NodeDisconnected += NetworkEventsHandlers.OnNodeDisconnected;
                Network.Events.ChangedNickname += NetworkEventsHandlers.OnChangedNickname;
                Network.Start();
            }
            catch (Exception e)
            {
                if (e is System.Net.Sockets.SocketException)
                {
                    Prompt.Log.Add(Properties.Resources._MultipleInstancesError);
                }
                else
                {
                    throw;
                }
            }

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
