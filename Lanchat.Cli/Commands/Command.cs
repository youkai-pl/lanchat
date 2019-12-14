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

        // Execute command
        public void Execute(string command)
        {
            // Split arguments
            string[] args = command.Split(' ');

            // Commands
            switch (args[0])
            {
                case "help":
                    Help();
                    break;

                case "exit":
                    Exit();
                    break;

                case "nick":
                    if (args[1] != null)
                    {
                        Nick(args[1]);
                    }
                    else
                    {
                        Prompt.Alert("Bad command syntax");
                    }
                    break;

                case "sr":
                    program.Network.Out.DestroyLanchat();
                    break;

                case "list":
                    List();
                    break;

                case "mute":
                    if (args[1] != null)
                    {
                        Mute(args[1]);
                    }
                    else
                    {
                        Prompt.Alert("Bad command syntax");
                    }
                    break;

                case "unmute":
                    if (args[1] != null)
                    {
                        Unmute(args[1]);
                    }
                    else
                    {
                        Prompt.Alert("Bad command syntax");
                    }
                    break;

                case "connect":
                    if (!(args[1] == null || args[2] == null))
                    {
                        Connect(args[1], args[2]);
                    }
                    else
                    {
                        Prompt.Alert("Bad command syntax");
                    }
                    break;

                default:
                    Prompt.Out("Bad command");
                    break;
            }
        }
    }
}