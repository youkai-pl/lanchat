using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Console = Colorful.Console;

namespace Lanchat.Console.Ui
{
    public class Prompt
    {
        // Command prompt symbol
        public static string promptChar = "> ";

        // Input buffer
        public static List<char> inputBuffer = new List<char>();

        // Welcome screen
        public static void Welcome()
        {
            Colorful.Console.Title = "Lanchat 2";
            Out("Lanchat " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        public event EventHandler<InputEventArgs> RecievedInput;

        protected virtual void OnRecievedInput(string input)
        {
            RecievedInput(this, new InputEventArgs() { Input = input });
        }

        // Read from prompt
        public void Init()
        {
            Colorful.Console.OutputEncoding = System.Text.Encoding.UTF8;
            Out("");

            while (true)
            {
                string input = ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    OnRecievedInput(input);
                }
            }
        }

        // Read line
        private static string ReadLine()
        {
            int curIndex = 0;

            do
            {
                ConsoleKeyInfo readKeyResult = Colorful.Console.ReadKey(true);

                // Handle enter
                if (readKeyResult.Key == ConsoleKey.Enter)
                {
                    try
                    {
                        string output = string.Join("", inputBuffer.ToArray());
                        return output;
                    }
                    finally
                    {
                        Rewrite(curIndex);
                        inputBuffer.Clear();
                    }
                }

                // Handle left arrow
                if (readKeyResult.Key == ConsoleKey.LeftArrow)
                {
                    if (curIndex > 0)
                    {
                        if ((inputBuffer.Count / Colorful.Console.WindowWidth) > 0)
                        {
                            Alert("Editing messages longer than the terminal width can cause some problems and is temporarily disabled");
                        }
                        else
                        {
                            curIndex--;
                            Colorful.Console.CursorLeft--;
                        }
                    }
                }

                // Handle right arrow
                if (readKeyResult.Key == ConsoleKey.RightArrow)
                {
                    if (inputBuffer.Count > curIndex)
                    {
                        curIndex++;
                        if (Colorful.Console.CursorLeft + 1 < Colorful.Console.WindowWidth)
                        {
                            Colorful.Console.CursorLeft++;
                        }
                        else
                        {
                            Colorful.Console.CursorTop++;
                            Colorful.Console.CursorLeft = 0;
                        }
                    }
                }

                // Handle backspace
                if (readKeyResult.Key == ConsoleKey.Backspace)
                {
                    if (curIndex > 0)
                    {
                        if (Colorful.Console.CursorLeft - 2 > 0)
                        {
                            inputBuffer.RemoveAt(curIndex - 1);
                            curIndex--;
                            Rewrite(curIndex);
                        }
                        else
                        {
                            ClearLine();
                            Colorful.Console.CursorTop--;
                            inputBuffer.RemoveAt(curIndex - 1);
                            curIndex--;
                            Rewrite(curIndex);
                        }
                    }
                }
                else
                // Handle all other keypresses
                {
                    if (!char.IsControl(readKeyResult.KeyChar))
                    {
                        inputBuffer.Insert(curIndex, readKeyResult.KeyChar);
                        curIndex++;
                        Rewrite(curIndex);
                    }
                }
            }
            while (true);
        }

        // Rewrite input in consle
        private static void Rewrite(int curIndex)
        {
            Colorful.Console.CursorVisible = false;
            int linesCount = (inputBuffer.Count / Colorful.Console.WindowWidth) + 1;

            // Move the cursor to the right line
            ClearLine();
            if (linesCount > 1)
            {
                Colorful.Console.CursorTop -= linesCount - 1;
            }

            Colorful.Console.Write(promptChar + string.Join("", inputBuffer.ToArray()));

            // If input has one line move cursor to specific  column
            if (curIndex + 2 < Colorful.Console.WindowWidth)
            {
                Colorful.Console.CursorLeft = curIndex + 2;
            }

            Colorful.Console.CursorVisible = true;
        }

        // Write in console
        public static void Out(string message, Color? color = null, string nickname = null)
        {
            int linesCount;

            // This is shit, i dont know whats going on here
            if (inputBuffer.Count > Colorful.Console.WindowWidth)
            {
                linesCount = (inputBuffer.Count / Colorful.Console.WindowWidth) + 1;
            }
            else
            {
                linesCount = (message.Length / Colorful.Console.WindowWidth) + 1;
            }

            if (linesCount > 1 && (Colorful.Console.CursorTop - linesCount - 1 > 0))
            {
                Colorful.Console.CursorTop -= linesCount - 1;
            }
            ClearLine();

            if (!string.IsNullOrEmpty(nickname))
            {
                Colorful.Console.Write(DateTime.Now.ToString("HH:mm:ss") + " ", Color.DimGray);
                Colorful.Console.Write(nickname + " ", Color.SteelBlue);
                Colorful.Console.WriteLine(message);
            }
            else
            {
                Colorful.Console.WriteLine(message, color ?? Color.White);
            }
            Colorful.Console.Write(promptChar + string.Join("", inputBuffer.ToArray()));
        }

        // Notice formatted out
        public static void Notice(string message)
        {
            Out("[#] " + message, Color.DodgerBlue);
        }

        // Alert formatted out
        public static void Alert(string message)

        {
            Out("[!] " + message, Color.OrangeRed);
        }

        // Query
        public static string Query(string query)
        {
            Colorful.Console.CursorLeft = 0;
            Colorful.Console.Write(query + " ", Color.LightGreen);
            string answer = Colorful.Console.ReadLine();
            Colorful.Console.CursorTop--;
            ClearLine();
            return answer;
        }

        // Show crash screen and stop program
        public static void CrashScreen(Exception e)
        {
            Alert(e.Message);
            Colorful.Console.ReadKey();
            Environment.Exit(1);
        }

        // Clear line
        private static void ClearLine()
        {
            int currentLineCursor = Colorful.Console.CursorTop;
            Colorful.Console.SetCursorPosition(0, Colorful.Console.CursorTop);
            Colorful.Console.Write(new string(' ', Colorful.Console.WindowWidth));
            Colorful.Console.SetCursorPosition(0, currentLineCursor);
        }
    }

    // Input event args
    public class InputEventArgs : EventArgs
    {
        public string Input { get; set; }
    }
}