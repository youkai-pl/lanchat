using System;
using System.Diagnostics;

public static class Prompt
{
    // variables
    private static bool read = true;

    // welcome screen
    public static void Welcome()
    {
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        string version = fvi.FileVersion;

        Console.Title = "Lanchat 2";
        Console.WriteLine("Lanchat " + version);
        Console.WriteLine("");
    }

    // read from prompt
    public static void Read()
    {
        while (read)
        {
            Console.Write("> ");

            // read input
            string promptInput = Console.ReadLine();
            Console.SetCursorPosition(2, Console.CursorTop - 1);
            ClearLine();

            if (!string.IsNullOrEmpty(promptInput))
            {
                // check is input command
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

    // write notice
    public static void Notice(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }

    // query
    public static string Query(string query, bool blank)
    {
        read = false;
        string response;
        do
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(query + ": ");
            Console.ForegroundColor = ConsoleColor.White;
            response = Console.ReadLine();
        } while (string.IsNullOrEmpty(response) || blank);
        read = true;
        return response;
    }

    // local methods
    private static void ClearLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }
}

