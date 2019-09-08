using System;
using System.Drawing;
using Console = Colorful.Console;
using System.Diagnostics;

public static class Prompt
{
    // variables
    private static string promptChar = ">";

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
        while (true)
        {

            Console.Write(promptChar + " ");

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
                    Message(lanchat.Properties.User.Default.nick, promptInput);
                }
            }
        }
    }

    // outputs
    public static void Out(string message, Color? color = null)
    {
        int currentTopCursor = Console.CursorTop;
        int currentLeftCursor = Console.CursorLeft;
        Console.MoveBufferArea(0, currentTopCursor, Console.WindowWidth, 1, 0, currentTopCursor + 1);
        Console.CursorTop = currentTopCursor;
        Console.CursorLeft = 0;
        Console.WriteLine(message, color ?? Color.White);
        Console.CursorTop = currentTopCursor + 1;
        Console.CursorLeft = currentLeftCursor;
    }

    public static void Message(string nickname, string message)
    {
        int currentTopCursor = Console.CursorTop;
        int currentLeftCursor = Console.CursorLeft;
        Console.MoveBufferArea(0, currentTopCursor, Console.WindowWidth, 1, 0, currentTopCursor + 1);
        Console.CursorTop = currentTopCursor;
        Console.CursorLeft = 0;
        Console.Write(DateTime.Now.ToString("HH:mm:ss") + " ", Color.DimGray);
        Console.Write(nickname + " ", Color.SteelBlue);
        Console.WriteLine(message);
        Console.CursorTop = currentTopCursor + 1;
        Console.CursorLeft = currentLeftCursor;
    }

    public static void Notice(string message)
    {
        Out("[#] " + message, Color.DodgerBlue);
    }

    public static void Alert(string message)

    {
        Out("[!] " + message, Color.OrangeRed);
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

