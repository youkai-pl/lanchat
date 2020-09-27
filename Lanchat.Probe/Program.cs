using System;

namespace Lanchat.Probe
{
    public static class Program
    {
        private static int _port;

        public static void Main(string[] args)
        {
            _port = 3645;
            
            Console.WriteLine("Lanchat Probe");
            Console.WriteLine("Press key: ");
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
                        throw new NotImplementedException();

                    default:
                        Console.CursorTop--;
                        break;
                }
            } while (true);
        }
    }
}