// Lanchat 2
// Let's all love lain

using Lanchat.Cli.Commands;
using Lanchat.Cli.Ui;
using Lanchat.Common.NetworkLib;
using System;
using System.Diagnostics;
using System.Threading;

namespace Lanchat.Cli.ProgramLib
{
    public class Program
    {
        // Properties
        public bool DebugMode { get; set; }
        public Network Network { get; set; }
        public Command Commands { get; set; }
        public Prompt Prompt { get; set; }

        public void Main()
        {
            // Check is debug enabled
            Debug.Assert(DebugMode = true);

            if (DebugMode)
            {
                // Trace listener
                Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
                Trace.WriteLine("Debug mode enabled");
            }

            // Load or create config file
            Config.Load();

            // Show welcome screen
            Prompt.Welcome();

            // Check nickname
            if (string.IsNullOrEmpty(Config.Nickname))
            {
                // If nickname is blank create new with up to 20 characters
                var nick = Prompt.Query("Nickname:");
                while (nick.Length > 20 && nick.Length != 0)
                {
                    Prompt.Alert("Nick cannot be blank or longer than 20 characters");
                    nick = Prompt.Query("Choose nickname:");
                }
                Config.Nickname = nick;
            }

            // Initialize commands module
            Commands = new Command(this);

            // Initialize event handlers
            var eventHandlers = new EventHandlers(this);

            // Initialize prompt
            Prompt = new Prompt();
            Prompt.RecievedInput += eventHandlers.OnRecievedInput;
            new Thread(Prompt.Init).Start();

            // Initialize network
            Network = new Network(Config.BroadcastPort, Config.Nickname, Config.HostPort);
            Network.Events.HostStarted += eventHandlers.OnHostStarted;
            Network.Events.ReceivedMessage += eventHandlers.OnRecievedMessage;
            Network.Events.NodeConnected += eventHandlers.OnNodeConnected;
            Network.Events.NodeDisconnected += eventHandlers.OnNodeDisconnected;
            Network.Events.NodeSuspended += eventHandlers.OnNodeSuspended;
            Network.Events.NodeResumed += eventHandlers.OnNodeResumed;
            Network.Events.ChangedNickname += eventHandlers.OnChangedNickname;

            // Start network
            Network.Start();
        }
    }
}