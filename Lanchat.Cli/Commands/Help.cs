using Lanchat.Cli.Ui;

namespace Lanchat.Cli.Commands
{
    public partial class Command
    {
        public static void Help()
        {
            // Prompt.Out("/connect <ip> <port> - quit lanchat");
            Prompt.Out("/exit - quit lanchat");
            Prompt.Out("/help - list of commands");
            Prompt.Out("/nick <nickname> - change nick");
            Prompt.Out("/mute <nickname> - mute user");
            Prompt.Out("/unmute <nickname> - unmute user");
            Prompt.Out("");
        }
    }
}