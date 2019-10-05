using lanchat.Terminal;
using System;

namespace lanchat.Commands
{
    public static class Command
    {
        public static void Execute(string command)
        {
            string[] args = command.Split(' ');

            // commands switch
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
            }
        }

        // methods
        private static void ShowHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("/exit - quit lanchat");
            Console.WriteLine("/help - list of commands");
            Console.WriteLine("/nick - change nick");
            Console.WriteLine("");
        }

        private static void SetNick(string nick)
        {
            if (!string.IsNullOrEmpty(nick))
            {
                lanchat.Properties.User.Default.nick = nick;
                lanchat.Properties.User.Default.Save();
                Prompt.Notice("Nickname changed");
            }
        }

        private static void Exit() => Environment.Exit(0);
    }
}