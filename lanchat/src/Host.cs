using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace lanchat.HostLib
{
    class Host
    {
        public static void Listen(int port)
        {
            // start server
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();

            // wait for connection
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();  //if a connection exists, the server will accept it

                NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages

                byte[] hello = new byte[100];   //any message must be serialized (converted to byte array)
                hello = Encoding.Default.GetBytes("hello world");  //conversion string => byte array

                ns.Write(hello, 0, hello.Length);     //sending the message

                while (client.Connected)  //while the client is connected, we look for incoming messages
                {
                    byte[] msg = new byte[1024];     //the messages arrive as byte array
                    ns.Read(msg, 0, msg.Length);   //the same networkstream reads the message sent by the client
                    Console.WriteLine(msg); //now , we write the message as string
                }
            }
        }
    }
}
