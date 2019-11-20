using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lanchat.Common.HostLib
{
    class Host
    {
        // Start broadcast
        public void Broadcast(UdpClient udpClient, object self, int port)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(self));
                    udpClient.Send(data, data.Length, "255.255.255.255", port);
                    Thread.Sleep(1000);
                }
            });
        }

        // Start host
        public void StartHost(int port)
        {
            // Create server
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveTimeout = -1
            };
            server.Bind(new IPEndPoint(IPAddress.Any, port));
            server.Listen(-1);

            // Start listening
            while (true)
            {
                Socket client = server.Accept();
                new Thread(() =>
                {
                    try { Process(client); } catch (Exception ex) { Console.WriteLine("Client connection processing error: " + ex.Message); }
                }).Start();
            }

            // Host client process
            void Process(Socket client)
            {
                //OnHostEvent(new Status("connected", IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString())), EventArgs.Empty);

                byte[] response;
                int received;

                while (true)
                {
                    // Receive message from the server:
                    response = new byte[client.ReceiveBufferSize];
                    received = client.Receive(response);
                    if (received == 0)
                    {
                        //OnHostEvent(new Status("disconnected", IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString())), EventArgs.Empty);
                        return;
                    }

                    List<byte> respBytesList = new List<byte>(response);
                    //Recieve(Encoding.UTF8.GetString(respBytesList.ToArray()), IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString()));
                }
            }
        }
    }
}
