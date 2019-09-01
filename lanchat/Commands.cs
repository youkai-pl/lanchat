using System;

public static class Commands
{
    public static void Execute(string command)
    {
        // commands
        if (command == "help")
        {
            ShowHelp();
        }
        if (command == "exit")
        {
            Exit();
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("/help - list of commands");
        Console.WriteLine("/exit - quit lanchat");
    }

    private static void Exit() => Environment.Exit(0);
}