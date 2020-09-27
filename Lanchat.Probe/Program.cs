﻿using System;

namespace Lanchat.Probe
{
    public static class Program
    {
        private static int _port;

        public static void Main(string[] args)
        {
            _port = 3645;
            
            Console.WriteLine("Lanchat debug probe");
            Console.WriteLine("");
            Console.WriteLine("Select mode by pressing key: ");
            Console.WriteLine("S - Only server");
            Console.WriteLine("C - Only client");
            Console.WriteLine("P - P2P");

            // Select option
            do
            {
                Console.Write(">");
                
                var option = Console.ReadKey();
                Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");

                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (option.Key)
                {
                    case ConsoleKey.S:
                        _ = new ServerMode(_port);
                        break;

                    case ConsoleKey.C:
                        _ = new ClientMode(_port);
                        break;

                    case ConsoleKey.P:
                        _ = new P2PMode();
                        break;

                    default:
                        Console.CursorTop--;
                        break;
                }
            } while (true);
            // ReSharper disable once FunctionNeverReturns
        }
    }
}