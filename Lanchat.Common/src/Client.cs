using Lanchat.Common.HostLib;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lanchat.Common.ClientLib
{
    public class Client
    {
        // Listen broadcast
        public void ListenBroadcast(UdpClient udpClient)
        {
            Task.Run(() =>
            {
                var from = new IPEndPoint(0, 0);
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);

                    // Try parse
                    try
                    {
                        var paperplane = JsonConvert.DeserializeObject<Paperplane>(Encoding.UTF8.GetString(recvBuffer));
                        RecievedBroadcast(paperplane, from.Address, EventArgs.Empty);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine("Paperplane parsing error: " + e.Message);
                    }
                }
            });
        }

        // Tcp client
        private TcpClient tcpclnt;
        private NetworkStream nwStream;

        // Connect
        public void Connect(IPAddress ip, int port, Handshake handshake)
        {
            // Create client and stream
            tcpclnt = new TcpClient(ip.ToString(), port);
            nwStream = tcpclnt.GetStream();

            // Send handshake
            byte[] bytesToSend = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(handshake));
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        // Client events
        public delegate void ClientEventHandler(params object[] arguments);
        public event ClientEventHandler RecievedBroadcast;

        // Recieved broadcast
        protected virtual void OnRecievedBroadcast(Paperplane paperplane, IPAddress sender, EventArgs e)
        {
            RecievedBroadcast(paperplane, sender, EventArgs.Empty);
        }
    }
}
