using System;
using System.Diagnostics;

public static class Prompt
{
    // Initialize prompt
    public static void Init()
    {
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        string version = fvi.FileVersion;

        Console.Title = "Lanchat 2";
        Console.WriteLine("Lanchat " + version);
        Console.WriteLine("");
        Console.Write("> ");

        while (true)
        {
            // Read input
            string promptInput = Console.ReadLine();
            Console.SetCursorPosition(2, Console.CursorTop - 1);
            ClearCurrentConsoleLine();

            if (!string.IsNullOrEmpty(promptInput))
            {
                // Check is input command
                if (promptInput.StartsWith("/"))
                {
                    string command = promptInput.Substring(1);
                    Commands.Execute(command);
                }

                // or message
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(lanchat.Properties.User.Default.nick + ": ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(promptInput);
                    Console.WriteLine();
                }
            }
            Console.Write("> ");
        }
    }

    public static void Notice(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }

    // Local methods
    private static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }
}

