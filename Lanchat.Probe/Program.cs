using System;
using System.Net;
using Lanchat.Core;

namespace Lanchat.Probe
{
    public static class Program
    {
        // Default port
        private const int Port = 3645;
        
        // Default server ip
        private const string IpAddress = "127.0.0.1";

        public static void Main(string[] args)
        {
            Console.WriteLine("Lanchat Probe");
            Console.WriteLine("Press S for server or press other key for client");
            
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
            var server = new Server(IPAddress.Any, Port);
            server.Start();
            Console.WriteLine($"Server started on port {Port}");

            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }
                server.Multicast(input);
            }
            
            Console.WriteLine("Stopping");
            server.Stop();
        }

        private static void Client()
        {
            Console.WriteLine($"Starting client on port {Port}");

            var client = new Client(IpAddress, Port);
            Console.WriteLine($"Client connecting to {IpAddress}");
            client.ConnectAsync();
            Console.WriteLine("Connected");
            
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