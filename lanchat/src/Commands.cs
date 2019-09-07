using System;

public static class Commands
{
    public static void Execute(string command)
    {
        string[] args = command.Split(' ');

        // Commands switch
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

    // Methods
    private static void ShowHelp()
    {
        Console.WriteLine("/help - list of commands");
        Console.WriteLine("/exit - quit lanchat");
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