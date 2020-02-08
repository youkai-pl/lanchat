// Lanchat 2
// Let's all love lain

using Lanchat.Common.NetworkLib;
using Lanchat.Console.Commands;
using Lanchat.Console.Ui;
using System.Diagnostics;
using System.Threading;

namespace Lanchat.Console.ProgramLib
{
    public class Program
    {
        private bool _DeugMode;
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
                    Trace.Listeners.Add(new TextWriterTraceListener(System.Console.Out));
                    Prompt.Notice("Debug mode enabled");
                }
                else
                {
                    Trace.Listeners.Clear();
                    Prompt.Notice("Debug mode disabled");
                }
            }
        }

        public Network Network { get; set; }
        public Prompt Prompt { get; set; }
        public void Start()
        {
            // Check is debug enabled
            Debug.Assert(DebugMode = true);

            Config.Load();
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
            Network.Start();
        }

        private static void Main(string[] args)
        {
            var program = new Program();
            program.Start();
        }
    }
}