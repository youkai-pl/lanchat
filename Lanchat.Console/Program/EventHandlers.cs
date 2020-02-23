using Lanchat.Common.NetworkLib;
using Lanchat.Console.Ui;
using System.Diagnostics;

namespace Lanchat.Console.ProgramLib
{
    public class EventHandlers
    {
        public EventHandlers(Program program)
        {
            this.program = program;
        }

        // Main program reference
        private readonly Program program;

        // Input
        public void OnRecievedInput(object o, InputEventArgs e)
        {
            var input = e.Input;

            // Check is input command
            if (input.StartsWith("/"))
            {
                program.Commands.Execute(input.Substring(1));
            }

            // Or message
            else
            {
                Prompt.Out(input, null, Config.Nickname);
                program.Network.Methods.SendAll(input);
            }
        }

        // Host started
        public void OnHostStarted(object o, HostStartedEventArgs e)
        {
            Trace.WriteLine($"[APP] Host started on port {e.Port}");
            if (!program.DebugMode)
            {
                Prompt.Notice($"Host started on port {e.Port}");
            }
        }

        // Recieved message
        public void OnRecievedMessage(object o, ReceivedMessageEventArgs e)
        {
            if (!program.DebugMode)
            {
                Prompt.Out(e.Content.Trim(), null, e.Nickname);
            }
        }

        // Node connection
        public void OnNodeConnected(object o, NodeConnectionStatusEventArgs e)
        {
            if (!program.DebugMode)
            {
                Prompt.Notice(e.Nickname + " connected");
            }

            if (Config.Muted.Exists(x => x.Equals(e.NodeIP)))
            {
                var node = program.Network.NodeList.Find(x => x.Ip.Equals(e.NodeIP));
                node.Mute = true;
            }
        }

        // Node disconnection
        public void OnNodeDisconnected(object o, NodeConnectionStatusEventArgs e)
        {
            if (!program.DebugMode)
            {
                Prompt.Notice(e.Nickname + " disconnected");
            }
        }

        // Node suspended
        public void OnNodeSuspended(object o, NodeConnectionStatusEventArgs e)
        {
            if (!program.DebugMode)
            {
                Prompt.Notice(e.Nickname + " suspended. Waiting for reconnect");
            }
        }

        // Node resumed
        public void OnNodeResumed(object o, NodeConnectionStatusEventArgs e)
        {
            if (!program.DebugMode)
            {
                Prompt.Notice(e.Nickname + " reconnected");
            }
        }

        // Changed nickname
        public void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            if (!program.DebugMode)
            {
                Prompt.Notice($"{e.OldNickname} changed nickname to {e.NewNickname}");
            }
        }
    }
}