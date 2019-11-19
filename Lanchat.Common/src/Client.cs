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
                        Paperplane paperplane = JsonConvert.DeserializeObject<Paperplane>(Encoding.UTF8.GetString(recvBuffer));
                        Trace.WriteLine($"Valid paperplane recived from: {from.Address}");
                        RecievedBroadcast(paperplane, from.Address, EventArgs.Empty);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine("Paperplane parsing error: " + e.Message);
                    }
                }
            });
        }

        // Client events
        public delegate void ClientEventHandler(params object[] arguments);
        public event ClientEventHandler RecievedBroadcast;

        // Recieved broadcast
        protected virtual void OnRecievedBroadcast(Paperplane paperplane, IPAddress sender, EventArgs e)
        {
            RecievedBroadcast(paperplane, sender, EventArgs.Empty);
        }

        // Paperplane class
        public class Paperplane
        {
            public int Port { get; set; }
            public Guid Id { get; set; }
        }
    }
}
