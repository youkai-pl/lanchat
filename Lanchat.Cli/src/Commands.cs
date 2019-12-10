using Lanchat.Cli.ConfigLib;
using Lanchat.Cli.PromptLib;
using System;

namespace Lanchat.Cli.Program
{
    public class Command
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

                default:
                    Prompt.Out("Bad command");
                    break;
            }
        }

        // Exit
        private void Exit() => Environment.Exit(0);

        // Change nickname
        private void SetNick(string nick)
        {
            if (!string.IsNullOrEmpty(nick) && nick.Length < 20)
            {
                Config.Nickname = nick;
                program.Network.Out.ChangeNickname(nick);
                Prompt.Notice("Nickname changed");
            }
            else
            {
                Prompt.Alert("Nick cannot be blank or longer than 20 characters");
            }
        }

        // Show help
        private void ShowHelp()
        {
            Prompt.Out("");
            Prompt.Out("/exit - quit lanchat");
            Prompt.Out("/help - list of commands");
            Prompt.Out("/nick - change nick");
            Prompt.Out("");
        }

        // Show list of peers
        private void List()
        {
            Prompt.Out($"Connected peers: {program.Network.NodeList.Count}");
            foreach (var item in program.Network.NodeList)
            {
                Prompt.Out($"{item.Nickname} ({item.Ip})");
            }
        }
    }
}