using System;
using System.Diagnostics;
using System.Threading;
using Lanchat.Terminal.Ui;

namespace Lanchat.Terminal
{
    class Program
    {
        internal static Config Config { get; set; }

        static void Main(string[] args)
        {
            if (Array.IndexOf(args, "-debug") > -1)
            {
                // Attach trace here
            }

            Config = Config.Load();

            new Thread(() =>
            {
                Prompt.Start();
            }).Start();
        }
    }
}
