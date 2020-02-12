// Lanchat 2
// Let's all love lain

using Lanchat.Common.NetworkLib;
using Lanchat.Console.Commands;
using Lanchat.Console.Ui;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Lanchat.Console.ProgramLib
{
    public class Program
    {
        private bool _DeugMode;
        private TraceListener consoleTraceListener;
        public Command Commands { get; set; }

        public bool DebugMode
        {
            get
            {
                return _DeugMode;
            }

            set
            {
                _DeugMode = value;
                if (value)
                {
                    consoleTraceListener = new TimeTraceListener(System.Console.Out);
                    Trace.Listeners.Add(consoleTraceListener);
                    Prompt.Notice("Debug mode enabled");
                }
                else
                {
                    Trace.Listeners.Remove(consoleTraceListener);
                    consoleTraceListener.Dispose();
                    Prompt.Notice("Debug mode disabled");
                }
            }
        }

        public Network Network { get; set; }
        public Prompt Prompt { get; set; }

        public void Start()
        {
            Debug.Assert(DebugMode = true);
            Config.Load();

            // Start logging
            Trace.Listeners.Add(new TimeTraceListener($"{Config.Path}{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log"));
            Trace.IndentSize = 11;
            Trace.AutoFlush = true;
            Trace.WriteLine("[APP] Logging started");

            // Delete old log files
            new Thread(() =>
            {
                foreach (var fi in new DirectoryInfo(Config.Path).GetFiles("*.log").OrderByDescending(x => x.LastWriteTime).Skip(5))
                {
                    fi.Delete();
                }
            }).Start();

            Prompt.Welcome();

            // Check nickname
            if (string.IsNullOrWhiteSpace(Config.Nickname))
            {
                var nick = Prompt.Query("Nickname:").Trim();

                while (nick.Length >= 20 || string.IsNullOrWhiteSpace(nick))
                {
                    Prompt.Alert("Nick cannot be blank or longer than 20 characters");
                    nick = Prompt.Query("Choose nickname:").Trim();
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
            Network.Start();
        }

        private static void Main(string[] args)
        {
            var program = new Program();
            program.Start();
        }

        public class TimeTraceListener : TextWriterTraceListener
        {
            public TimeTraceListener(string fileName) : base(fileName)
            {
            }

            public TimeTraceListener(TextWriter writer) : base(writer)
            {
            }

            public override void WriteLine(string message)
            {
                if (IndentLevel > 0)
                {
                    base.WriteLine(message);
                }
                else
                {
                    base.WriteLine(DateTime.Now.ToString("[HH:mm:ss] ") + message);
                }
            }
        }
    }
}