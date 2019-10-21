using lanchat.CommandsLib;
using System;
using System.Drawing;
using System.Reflection;
using static lanchat.Program;
using Console = Colorful.Console;

namespace lanchat.PromptLib
{
    public static class Prompt
    {
        // variables
        public static string promptChar = ">";
        public static string inputBuffer;

        // welcome screen
        public static void Welcome()
        {
            Console.Title = "Lanchat 2";
            Out("Lanchat " + GetVersion());
            Out("Main port: " + Config["mport"].ToString());
            Out("Broadcast port: " + Config["bport"].ToString());
        }

        // read from prompt
        public static void Init()
        {
            while (true)
            {
                string promptInput = ReadLine();
                inputBuffer = "";

                if (!string.IsNullOrEmpty(promptInput))
                {
                    // check is input command
                    if (promptInput.StartsWith("/"))
                    {
                        string command = promptInput.Substring(1);
                        Command.Execute(command);
                    }

                    // or message
                    else
                    {
                        Out(promptInput, null, Config["nickname"]);
                    }
                }
            }
        }

        private static string ReadLine()
        {
            inputBuffer = "";

            int curIndex = 0;
            do
            {
                ConsoleKeyInfo readKeyResult = Console.ReadKey(true);

                // handle Enter
                if (readKeyResult.Key == ConsoleKey.Enter)
                {
                    return inputBuffer;
                }

                // handle backspace
                if (readKeyResult.Key == ConsoleKey.Backspace)
                {
                    if (curIndex > 0)
                    {
                        inputBuffer = inputBuffer.Remove(inputBuffer.Length - 1);
                        Console.Write(readKeyResult.KeyChar);
                        Console.Write(' ');
                        Console.Write(readKeyResult.KeyChar);
                        curIndex--;
                    }
                }
                else
                // handle all other keypresses
                {
                    if (!char.IsControl(readKeyResult.KeyChar))
                    {
                        inputBuffer += readKeyResult.KeyChar;
                        Console.Write(readKeyResult.KeyChar);
                        curIndex++;
                    }  
                }
            }
            while (true);
        }

        public static void Out(string message, Color? color = null, string nickname = null)
        {
            ClearLine();
            if (!string.IsNullOrEmpty(nickname))
            {
                Console.Write(DateTime.Now.ToString("HH:mm:ss") + " ", Color.DimGray);
                Console.Write(nickname + " ", Color.SteelBlue);
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine(message, color ?? Color.White);
            }
            Console.Write(promptChar + " " + inputBuffer);
        }

        public static void Notice(string message)
        {
            Out("[#] " + message, Color.DodgerBlue);
        }

        public static void Alert(string message)

        {
            Out("[!] " + message, Color.OrangeRed);
        }

        public static string Query(string query)
        {
            Console.Write(query + " ", Color.LightGreen);
            string answer = Console.ReadLine();
            ClearLine();
            return answer;
        }

        private static void ClearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        private static string GetVersion()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
#if DEBUG
            version += " DEBUG MODE";
#endif
#if ALPHA
        version += " Alpha";
#endif
            return version;
        }
    }
}