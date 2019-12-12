using Lanchat.Cli.Ui;

namespace Lanchat.Cli.Commands
{
    public partial class Command
    {
        public static void ShowHelp()
        {
            Prompt.Out("/exit - quit lanchat");
            Prompt.Out("/help - list of commands");
            Prompt.Out("/nick - change nick");
            Prompt.Out("");
        }
    }
}
