using Lanchat.Console.Ui;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public static void Help()
        {
            Prompt.Out("/about");
            Prompt.Out("/connect <ip> <port>");
            Prompt.Out("/exit");
            Prompt.Out("/help");
            Prompt.Out("/mute <nickname>");
            Prompt.Out("/nick <nickname> ");
            Prompt.Out("/unmute <nickname>");
            Prompt.Out("");
        }
    }
}