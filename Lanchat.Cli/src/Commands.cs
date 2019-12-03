using Lanchat.Cli.ConfigLib;
using Lanchat.Cli.PromptLib;
using System;

namespace Lanchat.Cli.CommandsLib
{
    public static class Command
    {
        public static void Execute(string command, Program.Program program)
        {
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
                    SetNick(args[1], program);
                    break;

                default:
                    Prompt.Out("Bad command");
                    break;
            }
        }

        // Methods
        private static void ShowHelp()
        {
            Prompt.Out("");
            Prompt.Out("/exit - quit lanchat");
            Prompt.Out("/help - list of commands");
            Prompt.Out("/nick - change nick");
            Prompt.Out("");
        }

        private static void SetNick(string nick, Program.Program program)
        {
            if (!string.IsNullOrEmpty(nick) && nick.Length < 20)
            {
                Config.Edit("nickname", nick);
                program.network.ChangeNickname(nick);
                Prompt.Notice("Nickname changed");
            } else
            {
                Prompt.Alert("Nick cannot be blank or longer than 20 characters");
            }
        }

        private static void Exit() => Environment.Exit(0);
    }
}