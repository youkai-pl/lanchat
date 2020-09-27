using System;

namespace Lanchat.Probe
{
    public static class Program
    {
        private static int _port;
        private static string _ipAddress;

        public static void Main(string[] args)
        {
            Console.WriteLine("Lanchat Probe");
            Console.WriteLine("Press S for server or press other key for client");

            _port = 3645;
            _ipAddress = "127.0.0.1";

            var option = Console.ReadKey();
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");

            if (option.Key == ConsoleKey.S)
            {
                _ = new ServerMode(_port);
            }
            else
            {
                _ = new ClientMode(_ipAddress, _port);
            }
        }
    }
}