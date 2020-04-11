using System;
using System.Diagnostics;

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
        }
    }
}
