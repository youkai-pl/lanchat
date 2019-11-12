using lanchat.CommandsLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using static Lanchat.Program;
using Console = Colorful.Console;

namespace lanchat.Cli.PromptLib
{
    public static class Prompt
    {
        // command prompt symbol
        public static string promptChar = "> ";

        // input buffer
        public static List<char> inputBuffer = new List<char>();

        // welcome screen
        public static void Welcome()
        {
            Console.Title = "Lanchat 2";
            Out("Lanchat " + GetVersion());
            Out("Broadcast port: " + Config["bport"].ToString());
        }

        // read from prompt
        public static void Init()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                string promptInput = ReadLine();

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
            int curIndex = 0;

            do
            {
                ConsoleKeyInfo readKeyResult = Console.ReadKey(true);

                // handle enter
                if (readKeyResult.Key == ConsoleKey.Enter)
                {
                    try
                    {
                        string output = string.Join("", inputBuffer.ToArray());
                        return output;
                    }
                    finally
                    {
                        ResetCursor(curIndex);
                        inputBuffer.Clear();
                    }
                }

                // handle left arrow
                if (readKeyResult.Key == ConsoleKey.LeftArrow)
                {
                    if (curIndex > 0)
                    {

                        if ((inputBuffer.Count / Console.WindowWidth) > 0)
                        {
                            Alert("Editing messages longer than the terminal width can cause some problems and is temporarily disabled");
                        }
                        else
                        {
                            curIndex--;
                            Console.CursorLeft--;
                        }
                    }
                }

                // handle right arrow
                if (readKeyResult.Key == ConsoleKey.RightArrow)
                {
                    if (inputBuffer.Count > curIndex)
                    {
                        curIndex++;
                        if (Console.CursorLeft + 1 < Console.WindowWidth)
                        {
                            Console.CursorLeft++;
                        }
                        else
                        {
                            Console.CursorTop++;
                            Console.CursorLeft = 0;
                        }
                    }
                }

                // handle backspace
                if (readKeyResult.Key == ConsoleKey.Backspace)
                {
                    if (curIndex > 0)
                    {
                        if (Console.CursorLeft - 2 > 0)
                        {
                            inputBuffer.RemoveAt(curIndex - 1);
                            curIndex--;
                            ResetCursor(curIndex);
                        }
                        else
                        {
                            ClearLine();
                            Console.CursorTop--;
                            inputBuffer.RemoveAt(curIndex - 1);
                            curIndex--;
                            ResetCursor(curIndex);
                        }
                    }
                }
                else
                // handle all other keypresses
                {
                    if (!char.IsControl(readKeyResult.KeyChar))
                    {
                        inputBuffer.Insert(curIndex, readKeyResult.KeyChar);
                        curIndex++;
                        ResetCursor(curIndex);
                    }
                }
            }
            while (true);
        }

        private static void ResetCursor(int curIndex)
        {
            Console.CursorVisible = false;
            int linesCount = (inputBuffer.Count / Console.WindowWidth) + 1;

            // move the cursor to the right line
            ClearLine();
            if (linesCount > 1)
            {
                Console.CursorTop -= linesCount - 1;
            }

            Console.Write(promptChar + string.Join("", inputBuffer.ToArray()));

            // if input has one line move cursor to specific  column
            if (curIndex + 2 < Console.WindowWidth)
            {
                Console.CursorLeft = curIndex + 2;
            }

            Console.CursorVisible = true;
        }

        public static void Out(string message, Color? color = null, string nickname = null)
        {
            int linesCount;

            // this is shit, i dont know whats going on here
            if (inputBuffer.Count > Console.WindowWidth)
            {
                linesCount = (inputBuffer.Count / Console.WindowWidth) + 1;
            }
            else
            {
                linesCount = (message.Length / Console.WindowWidth) + 1;
            }

            if (linesCount > 1 && (Console.CursorTop - linesCount - 1 > 0))
            {
                Console.CursorTop -= linesCount - 1;
            }
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
            Console.Write(promptChar + string.Join("", inputBuffer.ToArray()));
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