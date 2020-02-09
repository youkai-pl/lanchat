using Lanchat.Console.ProgramLib;
using Lanchat.Console.Ui;
using System.Linq;

namespace Lanchat.Console.Commands
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
                    Help();
                    break;

                case "exit":
                    Exit();
                    break;

                case "nick":
                    if (args[1] != null)
                    {
                        Nick(string.Join(" ", args.Skip(1)));
                    }
                    else
                    {
                        Prompt.Alert("Bad command syntax");
                    }
                    break;

                case "list":
                    List();
                    break;

                case "mute":
                    if (args.Length > 1)
                    {
                        Mute(string.Join(" ", args.Skip(1)));
                    }
                    else
                    {
                        Prompt.Alert("Bad command syntax");
                    }
                    break;

                case "unmute":
                    if (args[1] != null)
                    {
                        Unmute(string.Join(" ", args.Skip(1)));
                    }
                    else
                    {
                        Prompt.Alert("Bad command syntax");
                    }
                    break;

                case "connect":
                    if (args.Length > 2)
                    {
                        Connect(args[1], args[2]);
                    }
                    else
                    {
                        Prompt.Alert("Bad command syntax");
                    }
                    break;

                case "debug":
                    Debug();
                    break;

                default:
                    Prompt.Out("Bad command");
                    break;
            }
        }
    }
}