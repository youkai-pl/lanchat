using Lanchat.Cli.ProgramLib;
using Lanchat.Cli.Ui;

namespace Lanchat.Cli.Commands
{
    public partial class Command
    {
        public Command(Program program)
        {
            this.program = program;
        }

        // Main program reference
        private readonly Program program;

        // Commands
        public void Execute(string command)
        {
            // Split arguments
            string[] args = command.Split(' ');

            // Commands
            switch (args[0])
            {
                case "help":
                    ShowHelp();
                    break;

                case "exit":
                    Exit();
                    break;

                case "nick":
                    SetNick(args[1]);
                    break;

                case "sr":
                    program.Network.Out.DestroyLanchat();
                    break;

                case "list":
                    List();
                    break;

                case "mute":
                    Mute(args[1]);
                    break;

                case "unmute":
                    Unmute(args[1]);
                    break;

                default:
                    Prompt.Out("Bad command");
                    break;
            }
        }
    }
}
