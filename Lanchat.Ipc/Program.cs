using System;

namespace Lanchat.Ipc
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Lansock");
            var socket = new SocketListener("lc.sock");
            socket.StartListening();
        }
    }
}