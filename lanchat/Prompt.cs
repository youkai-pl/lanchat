using System;
using System.Diagnostics;

public static class Prompt
{
    public static void Init()
    {
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        string version = fvi.FileVersion;

        Console.WriteLine("Lanchat " + version);
        Console.WriteLine("");
        Console.Write("> ");

        while (true)
        {
            // read input
            string promptInput = Console.ReadLine();
            Console.SetCursorPosition(2, Console.CursorTop - 1);
            ClearCurrentConsoleLine();

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
                    Console.Write("nick: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(promptInput);
                    Console.WriteLine();
                }
            }
            Console.Write("> ");
        }
    }

    // Functions
    private static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }
}

