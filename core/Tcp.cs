using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace lanchat.core.tcp
{
    class Tcp
    {
        public static void Host(int port)
        {
            // start server
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();

            // wait for connection
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received : " + dataReceived);
            }
        }

        public static void Connect(string ip, int port)
        {
            TcpClient tcpclnt = new TcpClient(ip, port);
            NetworkStream nwStream = tcpclnt.GetStream();

            string textToSend = "hello world";

            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

            Console.WriteLine("Sending : " + textToSend);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }
    }
}
