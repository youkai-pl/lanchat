using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lanchat.prompt
{
    public static class Prompt
    {
        public static void Init()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            Console.WriteLine("Lanchat Alpha " + version);
            Console.Write("> ");

            while (true)
            {
                string promptInput;
                promptInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(promptInput))
                {
                    Console.Write("> ");
                } else
                {
                    Console.SetCursorPosition(2, Console.CursorTop-1);
                }
            }
        }
    }
}
