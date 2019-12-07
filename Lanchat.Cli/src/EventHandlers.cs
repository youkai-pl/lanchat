using Lanchat.Cli.ConfigLib;
using Lanchat.Cli.PromptLib;
using Lanchat.Common.NetworkLib;

namespace Lanchat.Cli.Program
{
    public class EventHandlers
    {
        public EventHandlers(Program program)
        {
            this.program = program;
        }

        // Main program reference
        private readonly Program program;

        // Handle input
        public void OnRecievedInput(object o, InputEventArgs e)
        {
            var input = e.Input;

            // Check is input command
            if (input.StartsWith("/"))
            {
                program.Command.Execute(input.Substring(1));
            }

            // Or message
            else
            {
                Prompt.Out(input, null, Config.Nickname);
                program.Network.ApiOutputs.SendAll(input);
            }
        }

        // Handle message
        public void OnRecievedMessage(object o, RecievedMessageEventArgs e)
        {
            if (!program.DebugMode)
            {
                Prompt.Out(e.Content, null, e.Nickname);
            }
        }

        // Handle connect
        public void OnNodeConnected(object o, NodeConnectionStatusEvent e)
        {
            if (!program.DebugMode)
            {
                Prompt.Notice(e.Nickname + " connected");
            }
        }

        // Handle disconnect
        public void OnNodeDisconnected(object o, NodeConnectionStatusEvent e)
        {
            if (!program.DebugMode)
            {
                Prompt.Notice(e.Nickname + " disconnected");
            }
        }

        // Handle changed nickname
        public void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            if (!program.DebugMode)
            {
                Prompt.Notice($"{e.OldNickname} changed nickname to {e.NewNickname}");
            }
        }
    }
}