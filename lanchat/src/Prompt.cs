using System;
using System.Diagnostics;

public static class Prompt
{
    private static bool readPrompt = true;

    // Welcome screen
    public static void Welcome()
    {
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        string version = fvi.FileVersion;

        Console.Title = "Lanchat 2";
        Console.WriteLine("Lanchat " + version);
        Console.WriteLine("");
    }

    // Initialize prompt
    public static void Read()
    {
        while (readPrompt)
        {
            Console.Write("> ");

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
                    Console.Write(lanchat.Properties.User.Default.nick + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(promptInput);
                    Console.WriteLine();
                }
            }
        }
    }

    // Write notice
    public static void Notice(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }

    // Query
    public static string Query(string query, bool blank)
    {
        readPrompt = false;
        string response = null;
        do
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(query + ": ");
            Console.ForegroundColor = ConsoleColor.White;
            response = Console.ReadLine();
        } while (string.IsNullOrEmpty(response) || blank);
        readPrompt = true;
        return response;
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

