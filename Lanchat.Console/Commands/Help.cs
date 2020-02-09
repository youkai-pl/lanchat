using Lanchat.Console.Ui;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public static void Help()
        {
            // Prompt.Out("/connect <ip> <port> - quit lanchat");
            Prompt.Out("/exit");
            Prompt.Out("/help");
            Prompt.Out("/nick <nickname> ");
            Prompt.Out("/mute <nickname>");
            Prompt.Out("/unmute <nickname>");
            Prompt.Out("");
        }
    }
}