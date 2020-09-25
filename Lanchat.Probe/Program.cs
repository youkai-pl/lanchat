using System;
using System.Net;
using Lanchat.Core;

namespace Lanchat.Probe
{
    public static class Program
    {
        // Default port
        private const int Port = 3645;

        public static void Main(string[] args)
        {
            Console.WriteLine("Lanchat Probe");
            Console.WriteLine("Press S for server or press other key for client");
            
            var option = Console.ReadKey();
            if (option.Key == ConsoleKey.S)
            {
                Host();      
            }
        }

        private static void Host()
        {
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
            Console.WriteLine($"Starting server on port {Port}");
            var server = new Server(IPAddress.Any, Port);

            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }
                server.Multicast(input);
            }
            
            Console.WriteLine("Stopping.");
            server.Stop();
        }
    }
}