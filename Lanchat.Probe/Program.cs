using System;
using Lanchat.Core;

namespace Lanchat.Probe
{
    public static class Program
    {
        private static int _port;
        private static string _ipAddress;
        private static Network _network;
        private static EventsHandlers _eventsHandlers;

        public static void Main(string[] args)
        {
            Console.WriteLine("Lanchat Probe");
            Console.WriteLine("Press S for server or press other key for client");

            _port = 3645;
            _ipAddress = "127.0.0.1";
            _network = new Network(_port);
            _eventsHandlers = new EventsHandlers(_network.Events);

            var option = Console.ReadKey();
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");

            if (option.Key == ConsoleKey.S)
            {
                Server();
            }
            else
            {
                Client();
            }
        }

        private static void Server()
        {
            _network.Server.Start();
            Console.WriteLine($"Server started on port {_port}");

            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                _network.Server.Multicast(input);
            }

            Console.WriteLine("Stopping");
            _network.Server.Stop();
        }

        private static void Client()
        {
            Console.WriteLine($"Starting client on port {_port}");
            var client = _network.CreateClient(_ipAddress, _port);
            Console.WriteLine($"Client connecting to {_ipAddress}");
            client.Connect();
            
            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                client.SendAsync(input);
            }


            Console.WriteLine("Stopping");
            client.DisconnectAndStop();
        }
    }
}